// App.tsx
import React, { useState, useEffect } from "react";
import {
  Student,
  Faculty,
  StudentStatus,
  StudentRequest,
  program,
} from "./interface";
import { api, ApiResponse } from "./api";
import StudentListScreen from "./screens/StudentListScreen";
import StudentForm from "./components/StudentForm";
import "./App.css";

function App() {
  const [students, setStudents] = useState<Student[]>([]);
  const [faculties, setFaculties] = useState<Faculty[]>([]);
  const [statuses, setStatuses] = useState<StudentStatus[]>([]);
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<React.ReactNode | null>(null);
  const [filteredStudents, setFilteredStudents] = useState<Student[]>([]);
  const [currentScreen, setCurrentScreen] = useState<"list" | "form">("list");
  const [programs, setPrograms] = useState<program[]>([]);

  useEffect(() => {
    const loadInitialData = async () => {
      try {
        setIsLoading(true);
        const [facultiesData, statusesData, studentsData, programsData] =
          await Promise.all([
            api.getFaculties(),
            api.getStudentStatuses(),
            api.getStudents(),
            api.getPrograms(),
          ]);

        setFaculties(facultiesData.data || []);
        setStatuses(statusesData.data || []);
        setStudents(studentsData.data || []);
        setFilteredStudents(studentsData.data || []);
        setPrograms(programsData.data || []);
      } catch (err) {
        console.error("Error loading data:", err);
        setError(err instanceof Error ? err.message : "An error occurred");
      } finally {
        setIsLoading(false);
      }
    };

    loadInitialData();
  }, []);

  const handleAddStudent = async (
    student: StudentRequest
  ): Promise<ApiResponse<Student>> => {
    try {
      const response = await api.addStudent(student);
      if (response.succeeded) {
        const studentsData = await api.getStudents();
        setStudents(studentsData.data || []);
        setFilteredStudents(studentsData.data || []);
        setIsFormOpen(false);
        setCurrentScreen("list");
      }
      return response;
    } catch (err) {
      console.error("Error adding student:", err);
      const errorMessage =
        err instanceof Error ? err.message : "Failed to add student";
      setError(errorMessage);
      setTimeout(() => setError(null), 5000);
      throw err;
    }
  };

  const handleDeleteStudent = async (mssv: string) => {
    try {
      await api.deleteStudent(mssv);
      setStudents(students.filter((student) => student.studentId !== mssv));
      setFilteredStudents(
        filteredStudents.filter((student) => student.studentId !== mssv)
      );
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to delete student");
    }
  };

  const handleUpdateStudent = async (
    updatedStudent: StudentRequest
  ): Promise<ApiResponse<Student>> => {
    try {
      const response = await api.updateStudent(updatedStudent);
      if (response.succeeded) {
        // Refresh the student list if needed
        const studentsData = await api.getStudents();
        setStudents(studentsData.data || []);
        setFilteredStudents(studentsData.data || []);
        setSelectedStudent(null);
        setIsFormOpen(false);
        setCurrentScreen("list"); // Redirect to home (list screen)
      }
      return response;
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to update student");
      throw err;
    }
  };

  const handleSearch = async (studentId: string) => {
    try {
      if (!studentId.trim()) {
        setFilteredStudents(students);
        return;
      }
      const studentResponse = await api.getStudentById(studentId);
      setFilteredStudents(studentResponse.data ? [studentResponse.data] : []);
    } catch (err) {
      console.error("Error searching student:", err);
      setFilteredStudents([]);
    }
  };

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div className="app">
      {currentScreen === "list" && (
        <StudentListScreen
          students={filteredStudents}
          onAddNew={() => {
            setSelectedStudent(null);
            setIsFormOpen(true);
            setCurrentScreen("form");
          }}
          onDelete={handleDeleteStudent}
          onEdit={(student) => {
            setSelectedStudent(student);
            setIsFormOpen(true);
            setCurrentScreen("form");
          }}
          onSearch={handleSearch}
        />
      )}

      {currentScreen === "form" && (
        <StudentForm
          student={selectedStudent}
          faculties={faculties}
          statuses={statuses}
          programs={programs}
          error={error}
          onSubmit={async (student) => {
            if (selectedStudent) {
              return await handleUpdateStudent(student);
            } else {
              return await handleAddStudent(student);
            }
          }}
          onClose={() => {
            setIsFormOpen(false);
            setSelectedStudent(null);
            setError(null);
            setCurrentScreen("list");
          }}
        />
      )}
    </div>
  );
}

export default App;

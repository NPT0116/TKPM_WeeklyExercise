import React, { useState, useEffect } from "react";
import {
  Student,
  Faculty,
  StudentStatus,
  StudentRequest,
  program,
} from "./interface";
import { api } from "./api";
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
  const [error, setError] = useState<string | null>(null);
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

        console.log("Loaded faculties:", facultiesData);
        console.log("Loaded statuses:", statusesData);
        console.log("Loaded programs:", programsData);

        setFaculties(facultiesData);
        setStatuses(statusesData);
        setStudents(studentsData);
        setFilteredStudents(studentsData);
        setPrograms(programsData);
      } catch (err) {
        console.error("Error loading data:", err);
        setError(err instanceof Error ? err.message : "An error occurred");
      } finally {
        setIsLoading(false);
      }
    };

    loadInitialData();
  }, []);

  const handleAddStudent = async (student: StudentRequest) => {
    try {
      await api.addStudent(student);
      console.log("Student added successfully.");

      // Reload the students from the API
      const studentsData = await api.getStudents();
      setStudents(studentsData);
      setFilteredStudents(studentsData);
      setIsFormOpen(false);
      setCurrentScreen("list");
      return true;
    } catch (err) {
      console.error("Error adding student:", err);
      const errorMessage =
        err instanceof Error ? err.message : "Failed to add student";
      setError(errorMessage);
      const formattedError = errorMessage
        .split("\n")
        .map((line, i) => <div key={i}>{line}</div>);
      setError(formattedError);
      setTimeout(() => setError(null), 5000);
      return false;
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

  const handleUpdateStudent = async (updatedStudent: StudentRequest) => {
    try {
      const result = await api.updateStudent(updatedStudent);
      // Update both arrays with the complete student data
      const updateStudentInList = (student: Student) =>
        student.studentId === result.studentId ? result : student;

      setStudents(students.map(updateStudentInList));
      setFilteredStudents(filteredStudents.map(updateStudentInList));
      setSelectedStudent(null);
      setIsFormOpen(false);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to update student");
    }
  };

  const handleSearch = async (studentId: string) => {
    try {
      if (!studentId.trim()) {
        setFilteredStudents(students);
        return;
      }
      const student = await api.getStudentById(studentId);
      setFilteredStudents(student ? [student] : []);
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
              await handleUpdateStudent(student);
              setCurrentScreen("list");
            } else {
              const success = await handleAddStudent(student);
              if (success) {
                setCurrentScreen("list");
              }
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

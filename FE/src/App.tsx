// src/App.tsx
import React, { useState, useEffect } from "react";
import {
  Student,
  Faculty,
  StudentStatus,
  StudentRequest,
  program,
} from "./interface";
import { api, ApiResponse } from "./api";
import NavigationBar, { Tab } from "./components/NavigationBar";
import StudentSection from "./components/StudentSection";
import FacultySection from "./components/FacultySection";
import StudentStatusSection from "./components/StudentStatusSection";
import ProgramSection from "./components/ProgramSection";
import ErrorPopup from "./components/ErrorPopup";
import hcmusLogo from "./assets/hcmus-logo-tkpm.png";
import "./App.css";

function App() {
  // --- State for Students ---
  const [students, setStudents] = useState<Student[]>([]);
  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);

  // --- State for Faculties ---
  const [faculties, setFaculties] = useState<Faculty[]>([]);
  const [selectedFaculty, setSelectedFaculty] = useState<Faculty | null>(null);

  // --- State for Student Statuses ---
  const [statuses, setStatuses] = useState<StudentStatus[]>([]);
  const [selectedStatus, setSelectedStatus] = useState<StudentStatus | null>(
    null
  );

  // --- State for Programs ---
  const [programs, setPrograms] = useState<program[]>([]);
  const [selectedProgram, setSelectedProgram] = useState<program | null>(null);

  // --- General state ---
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | React.ReactNode | null>(null);
  const [currentTab, setCurrentTab] = useState<Tab>("students");
  const [isFormOpen, setIsFormOpen] = useState(false);

  // Load initial data
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

  // --- Handlers for Students ---
  const handleAddStudent = async (
    student: StudentRequest
  ): Promise<ApiResponse<Student>> => {
    try {
      const response = await api.addStudent(student);
      if (response.succeeded) {
        const studentsData = await api.getStudents();
        setStudents(studentsData.data || []);
        setIsFormOpen(false);
        setSelectedStudent(null);
      }
      return response;
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to add student");
      throw err;
    }
  };

  const handleUpdateStudent = async (
    student: StudentRequest
  ): Promise<ApiResponse<Student>> => {
    try {
      const response = await api.updateStudent(student);
      if (response.succeeded) {
        const studentsData = await api.getStudents();
        setStudents(studentsData.data || []);
        setIsFormOpen(false);
        setSelectedStudent(null);
      }
      return response;
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to update student");
      throw err;
    }
  };

  const handleDeleteStudent = async (studentId: string) => {
    try {
      const response = await api.deleteStudent(studentId);
      if (response.succeeded) {
        const studentsData = await api.getStudents();
        setStudents(studentsData.data || []);
        setIsFormOpen(false);
        setSelectedStudent(null);
      } else {
        setError(response.message || response.errors);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to delete student");
    }
  };

  // --- Handlers for Faculties ---
  const handleAddFaculty = async (faculty: Faculty) => {
    try {
      const response = await api.addFaculty(faculty);
      if (response.succeeded) {
        const facultiesData = await api.getFaculties();
        setFaculties(facultiesData.data || []);
        setIsFormOpen(false);
        setSelectedFaculty(null);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to add faculty");
    }
  };

  const handleUpdateFaculty = async (faculty: Faculty) => {
    try {
      const response = await api.updateFaculty(faculty);
      if (response.succeeded) {
        const facultiesData = await api.getFaculties();
        setFaculties(facultiesData.data || []);
        setIsFormOpen(false);
        setSelectedFaculty(null);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to update faculty");
    }
  };

  const handleDeleteFaculty = async (facultyId: number) => {
    try {
      var response = await api.deleteFaculty(facultyId);
      if (response.succeeded) {
        setFaculties(faculties.filter((f) => f.facultyId !== facultyId));
      } else {
        setError(response.message || response.errors);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to delete faculty");
    }
  };

  // --- Handlers for Student Statuses ---
  const handleAddStatus = async (status: StudentStatus) => {
    try {
      const response = await api.addStudentStatus(status);
      if (response.succeeded) {
        const statusesData = await api.getStudentStatuses();
        setStatuses(statusesData.data || []);
        setIsFormOpen(false);
        setSelectedStatus(null);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to add status");
    }
  };

  const handleUpdateStatus = async (status: StudentStatus) => {
    try {
      const response = await api.updateStudentStatus(status);
      if (response.succeeded) {
        const statusesData = await api.getStudentStatuses();
        setStatuses(statusesData.data || []);
        setIsFormOpen(false);
        setSelectedStatus(null);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to update status");
    }
  };

  const handleDeleteStatus = async (statusId: number) => {
    try {
      var response = await api.deleteStudentStatus(statusId);
      if (response.succeeded) {
        setStatuses(statuses.filter((s) => s.statusId !== statusId));
      } else {
        setError(response.message || response.errors);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to delete status");
    }
  };

  // --- Handlers for Programs ---
  const handleAddProgram = async (prog: program) => {
    try {
      const response = await api.addProgram(prog);
      if (response.succeeded) {
        const programsData = await api.getPrograms();
        setPrograms(programsData.data || []);
        setIsFormOpen(false);
        setSelectedProgram(null);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to add program");
    }
  };

  const handleUpdateProgram = async (prog: program) => {
    try {
      const response = await api.updateProgram(prog);
      if (response.succeeded) {
        const programsData = await api.getPrograms();
        setPrograms(programsData.data || []);
        setIsFormOpen(false);
        setSelectedProgram(null);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to update program");
    }
  };

  const handleDeleteProgram = async (programId: number) => {
    try {
      var response = await api.deleteProgram(programId);
      if (response.succeeded) {
        setPrograms(programs.filter((p) => p.programId !== programId));
      } else {
        setError(response.message || response.errors);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to delete program");
    }
  };

  // Handler for closing the error modal
  const closeErrorPopup = () => {
    setError(null);
  };

  // Handler for student search by MSSV and other criteria
  const handleStudentSearch = async (params: {
    mssv?: string;
    facultyId?: number | null;
    name?: string;
  }) => {
    try {
      if (params.mssv && params.mssv.trim()) {
        // Search by MSSV
        const response = await api.getStudentById(params.mssv);
        setStudents(response.data ? [response.data] : []);
      } else if (
        params.facultyId !== null &&
        params.name &&
        params.name.trim()
      ) {
        // Search by faculty and student name
        const response = await api.searchStudents(
          params.facultyId,
          params.name
        );
        setStudents(response.data || []);
      } else if (params.facultyId !== null) {
        // Search by faculty only
        const response = await api.getStudentsByFacultyId(params.facultyId);
        setStudents(response.data || []);
      } else if (params.name && params.name.trim()) {
        // Search by student name only
        const response = await api.searchStudents(null, params.name);
        setStudents(response.data || []);
      } else {
        // If no criteria, reload all students
        const response = await api.getStudents();
        setStudents(response.data || []);
      }
    } catch (err) {
      console.error("Error searching students:", err);
      setError(err instanceof Error ? err.message : "Search failed");
    }
  };

  // Handler for import success (e.g., re-fetch students after import)
  const handleImportSuccess = async () => {
    const studentsData = await api.getStudents();
    setStudents(studentsData.data || []);
  };

  if (isLoading) return <div>Loading...</div>;

  return (
    <div className="app">
      {/* Logo in the top corner */}
      <header className="app-header">
        <img src={hcmusLogo} alt="HCMUS Logo" className="app-logo" />
      </header>

      {/* Error Popup */}
      {error && (
        <ErrorPopup message={error as string} onClose={closeErrorPopup} />
      )}

      {/* Navigation */}
      <NavigationBar
        currentTab={currentTab}
        onChangeTab={(tab) => {
          setCurrentTab(tab);
          setIsFormOpen(false);
        }}
      />

      {/* Render the appropriate section based on the current tab */}
      {currentTab === "students" && (
        <StudentSection
          onAddStudent={handleAddStudent}
          students={students}
          faculties={faculties}
          statuses={statuses}
          programs={programs}
          selectedStudent={selectedStudent}
          isFormOpen={isFormOpen}
          error={error}
          onUpdateStudent={handleUpdateStudent}
          onDeleteStudent={handleDeleteStudent}
          onCloseForm={() => {
            setIsFormOpen(false);
            setSelectedStudent(null);
          }}
          onEditStudent={(student) => {
            setSelectedStudent(student);
            setIsFormOpen(true);
          }}
          onImportSuccess={handleImportSuccess}
          onOpenAddForm={() => {
            setSelectedStudent(null);
            setIsFormOpen(true);
          }}
          onSearch={handleStudentSearch}
        />
      )}
      {currentTab === "faculties" && (
        <FacultySection
          faculties={faculties}
          selectedFaculty={selectedFaculty}
          isFormOpen={isFormOpen}
          error={error}
          onAddFaculty={handleAddFaculty}
          onUpdateFaculty={handleUpdateFaculty}
          onDeleteFaculty={handleDeleteFaculty}
          onCloseForm={() => {
            setIsFormOpen(false);
            setSelectedFaculty(null);
          }}
          onEditFaculty={(faculty) => {
            setSelectedFaculty(faculty);
            setIsFormOpen(true);
          }}
        />
      )}
      {currentTab === "statuses" && (
        <StudentStatusSection
          statuses={statuses}
          selectedStatus={selectedStatus}
          isFormOpen={isFormOpen}
          error={error}
          onAddStatus={handleAddStatus}
          onUpdateStatus={handleUpdateStatus}
          onDeleteStatus={handleDeleteStatus}
          onCloseForm={() => {
            setIsFormOpen(false);
            setSelectedStatus(null);
          }}
          onEditStatus={(status) => {
            setSelectedStatus(status);
            setIsFormOpen(true);
          }}
        />
      )}
      {currentTab === "programs" && (
        <ProgramSection
          programs={programs}
          selectedProgram={selectedProgram}
          isFormOpen={isFormOpen}
          error={error}
          onAddProgram={handleAddProgram}
          onUpdateProgram={handleUpdateProgram}
          onDeleteProgram={handleDeleteProgram}
          onCloseForm={() => {
            setIsFormOpen(false);
            setSelectedProgram(null);
          }}
          onEditProgram={(prog) => {
            setSelectedProgram(prog);
            setIsFormOpen(true);
          }}
        />
      )}
    </div>
  );
}

export default App;

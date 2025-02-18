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
import StudentListScreen from "./screens/StudentListScreen";
import StudentForm from "./components/StudentForm";
import FacultyListScreen from "./screens/FacultyListScreen";
import FacultyForm from "./components/FacultyForm";
import StudentStatusListScreen from "./screens/StudentStatusListScreen";
import StudentStatusForm from "./components/StudentStatusForm";
import ProgramListScreen from "./screens/ProgramListScreen";
import ProgramForm from "./components/ProgramForm";
import "./App.css";
import StudentSearchPanel from "./components/StudentSearchPanel";
import ImportExportPanel from "./components/ImportExportPanel";
type Tab = "students" | "faculties" | "statuses" | "programs";

function App() {
  // --- State cho Sinh viên ---
  const [students, setStudents] = useState<Student[]>([]);
  const [selectedStudent, setSelectedStudent] = useState<Student | null>(null);

  // --- State cho Khoa ---
  const [faculties, setFaculties] = useState<Faculty[]>([]);
  const [selectedFaculty, setSelectedFaculty] = useState<Faculty | null>(null);

  // --- State cho Tình trạng sinh viên ---
  const [statuses, setStatuses] = useState<StudentStatus[]>([]);
  const [selectedStatus, setSelectedStatus] = useState<StudentStatus | null>(
    null
  );

  // --- State cho Chương trình ---
  const [programs, setPrograms] = useState<program[]>([]);
  const [selectedProgram, setSelectedProgram] = useState<program | null>(null);

  // --- Các state chung ---
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<React.ReactNode | null>(null);
  const [currentTab, setCurrentTab] = useState<Tab>("students");
  const [isFormOpen, setIsFormOpen] = useState(false);
  useEffect(() => {
    const fetchData = async () => {
      try {
        if (currentTab === "students") {
          const studentsData = await api.getStudents();
          setStudents(studentsData.data || []);
        } else if (currentTab === "faculties") {
          const facultiesData = await api.getFaculties();
          setFaculties(facultiesData.data || []);
        } else if (currentTab === "statuses") {
          const statusesData = await api.getStudentStatuses();
          setStatuses(statusesData.data || []);
        } else if (currentTab === "programs") {
          const programsData = await api.getPrograms();
          setPrograms(programsData.data || []);
        }
      } catch (err) {
        console.error("Error loading data:", err);
        setError(err instanceof Error ? err.message : "An error occurred");
      }
    };

    fetchData();
  }, [currentTab]);

  useEffect(() => {
    const loadInitialData = async () => {
      try {
        setIsLoading(true);
        // Tải dữ liệu cho tất cả các màn hình cùng lúc
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

  // --- Handlers cho Sinh viên ---
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
    updatedStudent: StudentRequest
  ): Promise<ApiResponse<Student>> => {
    try {
      const response = await api.updateStudent(updatedStudent);
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
      await api.deleteStudent(studentId);
      setStudents(students.filter((s) => s.studentId !== studentId));
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to delete student");
    }
  };

  // --- Handlers cho Khoa ---
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
      await api.deleteFaculty(facultyId);
      setFaculties(faculties.filter((f) => f.facultyId !== facultyId));
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to delete faculty");
    }
  };

  // --- Handlers cho Tình trạng sinh viên ---
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
      await api.deleteStudentStatus(statusId);
      setStatuses(statuses.filter((s) => s.statusId !== statusId));
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to delete status");
    }
  };

  // --- Handlers cho Chương trình ---
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
      await api.deleteProgram(programId);
      setPrograms(programs.filter((p) => p.programId !== programId));
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to delete program");
    }
  };

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  const handleStudentSearch = async (params: {
    mssv?: string;
    facultyId?: number | null;
    name?: string;
  }) => {
    try {
      if (params.mssv && params.mssv.trim()) {
        // Search by MSSV (existing behavior)
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
        // Search by student name only (without faculty)
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

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div className="app">
      {/* Navigation */}
      <nav>
        <button
          onClick={() => {
            setCurrentTab("students");
            setIsFormOpen(false);
          }}
        >
          Sinh viên
        </button>
        <button
          onClick={() => {
            setCurrentTab("faculties");
            setIsFormOpen(false);
          }}
        >
          Khoa
        </button>
        <button
          onClick={() => {
            setCurrentTab("statuses");
            setIsFormOpen(false);
          }}
        >
          Tình trạng
        </button>
        <button
          onClick={() => {
            setCurrentTab("programs");
            setIsFormOpen(false);
          }}
        >
          Chương trình
        </button>
      </nav>

      {/* Student Screens */}
      {currentTab === "students" && !isFormOpen && (
        <>
          <StudentSearchPanel
            faculties={faculties}
            onSearch={handleStudentSearch}
          />
          <StudentListScreen
            students={students}
            onAddNew={() => {
              setSelectedStudent(null);
              setIsFormOpen(true);
            }}
            onEdit={(student) => {
              setSelectedStudent(student);
              setIsFormOpen(true);
            }}
            onDelete={handleDeleteStudent}
            onSearch={async (studentId) => {
              // This prop can be left empty or used for legacy MSSV search if desired
            }}
          />
          <ImportExportPanel />
        </>
      )}
      {currentTab === "students" && isFormOpen && (
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
          }}
        />
      )}
      {/* Màn hình quản lý Khoa */}
      {currentTab === "faculties" && !isFormOpen && (
        <FacultyListScreen
          faculties={faculties}
          onAddNew={() => {
            setSelectedFaculty(null);
            setIsFormOpen(true);
          }}
          onEdit={(faculty) => {
            setSelectedFaculty(faculty);
            setIsFormOpen(true);
          }}
          onDelete={handleDeleteFaculty}
        />
      )}
      {currentTab === "faculties" && isFormOpen && (
        <FacultyForm
          faculty={selectedFaculty}
          onSubmit={(faculty) => {
            if (selectedFaculty) {
              handleUpdateFaculty(faculty);
            } else {
              handleAddFaculty(faculty);
            }
          }}
          onClose={() => {
            setIsFormOpen(false);
            setSelectedFaculty(null);
          }}
        />
      )}

      {/* Màn hình quản lý Tình trạng sinh viên */}
      {currentTab === "statuses" && !isFormOpen && (
        <StudentStatusListScreen
          statuses={statuses}
          onAddNew={() => {
            setSelectedStatus(null);
            setIsFormOpen(true);
          }}
          onEdit={(status) => {
            setSelectedStatus(status);
            setIsFormOpen(true);
          }}
          onDelete={handleDeleteStatus}
        />
      )}
      {currentTab === "statuses" && isFormOpen && (
        <StudentStatusForm
          status={selectedStatus}
          onSubmit={(status) => {
            if (selectedStatus) {
              handleUpdateStatus(status);
            } else {
              handleAddStatus(status);
            }
          }}
          onClose={() => {
            setIsFormOpen(false);
            setSelectedStatus(null);
          }}
        />
      )}

      {/* Màn hình quản lý Chương trình */}
      {currentTab === "programs" && !isFormOpen && (
        <ProgramListScreen
          programs={programs}
          onAddNew={() => {
            setSelectedProgram(null);
            setIsFormOpen(true);
          }}
          onEdit={(prog) => {
            setSelectedProgram(prog);
            setIsFormOpen(true);
          }}
          onDelete={handleDeleteProgram}
        />
      )}
      {currentTab === "programs" && isFormOpen && (
        <ProgramForm
          program={selectedProgram}
          onSubmit={(prog) => {
            if (selectedProgram) {
              handleUpdateProgram(prog);
            } else {
              handleAddProgram(prog);
            }
          }}
          onClose={() => {
            setIsFormOpen(false);
            setSelectedProgram(null);
          }}
        />
      )}
    </div>
  );
}

export default App;

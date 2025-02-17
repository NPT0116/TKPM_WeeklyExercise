// src/api.ts
import {
  Student,
  Faculty,
  StudentStatus,
  StudentRequest,
  program,
} from "./interface";

const API_BASE_URL = "http://localhost:5230/api";

export interface ApiResponse<T> {
  data: T | null;
  succeeded: boolean;
  errors?: string[];
  message?: string;
}

const parseApiResponse = async <T>(
  response: Response
): Promise<ApiResponse<T>> => {
  try {
    const text = await response.text();
    if (!text) {
      // 204 No Content case
      return { data: null, succeeded: true, errors: [], message: "No Content" };
    }
    const responseData = JSON.parse(text);
    if (!response.ok) {
      return {
        data: null,
        succeeded: false,
        errors: responseData.errors || [],
        message: responseData.message || "An error occurred",
      };
    }
    return {
      data: responseData.data,
      succeeded: true,
      errors: [],
      message: "",
    };
  } catch (e) {
    throw new Error("Failed to parse response: " + e);
  }
};

export const api = {
  // --- Student Endpoints ---
  getStudents: async (): Promise<ApiResponse<Student[]>> => {
    const response = await fetch(`${API_BASE_URL}/students`);
    return parseApiResponse<Student[]>(response);
  },
  getStudentById: async (id: string): Promise<ApiResponse<Student>> => {
    const response = await fetch(`${API_BASE_URL}/students/${id}`);
    return parseApiResponse<Student>(response);
  },
  addStudent: async (
    student: StudentRequest
  ): Promise<ApiResponse<Student>> => {
    const response = await fetch(`${API_BASE_URL}/students`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(student),
    });
    return parseApiResponse<Student>(response);
  },
  updateStudent: async (
    student: StudentRequest
  ): Promise<ApiResponse<Student>> => {
    const response = await fetch(
      `${API_BASE_URL}/students/${student.studentId}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(student),
      }
    );
    return parseApiResponse<Student>(response);
  },
  deleteStudent: async (id: string): Promise<ApiResponse<string>> => {
    const response = await fetch(`${API_BASE_URL}/students/${id}`, {
      method: "DELETE",
    });
    return parseApiResponse<string>(response);
  },

  // --- New Search Endpoints ---

  // Search by faculty and/or student name (both query parameters are optional)
  searchStudents: async (
    facultyId: number | null,
    name: string
  ): Promise<ApiResponse<Student[]>> => {
    const queryParams = new URLSearchParams();
    if (facultyId !== null)
      queryParams.append("facultyId", facultyId.toString());
    if (name.trim() !== "") queryParams.append("name", name);
    const response = await fetch(
      `${API_BASE_URL}/students/search?${queryParams.toString()}`
    );
    return parseApiResponse<Student[]>(response);
  },

  // Get students by faculty id only
  getStudentsByFacultyId: async (
    facultyId: number
  ): Promise<ApiResponse<Student[]>> => {
    const response = await fetch(
      `${API_BASE_URL}/students/faculty/${facultyId}`
    );
    return parseApiResponse<Student[]>(response);
  },

  // --- Faculty, StudentStatus, Program endpoints remain unchanged ---
  getFaculties: async (): Promise<ApiResponse<Faculty[]>> => {
    const response = await fetch(`${API_BASE_URL}/Faculty`);
    return parseApiResponse<Faculty[]>(response);
  },
  addFaculty: async (faculty: Faculty): Promise<ApiResponse<Faculty>> => {
    const response = await fetch(`${API_BASE_URL}/Faculty`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(faculty),
    });
    return parseApiResponse<Faculty>(response);
  },
  updateFaculty: async (faculty: Faculty): Promise<ApiResponse<Faculty>> => {
    const response = await fetch(
      `${API_BASE_URL}/Faculty/${faculty.facultyId}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(faculty),
      }
    );
    return parseApiResponse<Faculty>(response);
  },
  deleteFaculty: async (id: number): Promise<ApiResponse<string>> => {
    const response = await fetch(`${API_BASE_URL}/Faculty/${id}`, {
      method: "DELETE",
    });
    return parseApiResponse<string>(response);
  },
  getStudentStatuses: async (): Promise<ApiResponse<StudentStatus[]>> => {
    const response = await fetch(`${API_BASE_URL}/StudentStatus`);
    return parseApiResponse<StudentStatus[]>(response);
  },
  addStudentStatus: async (
    status: StudentStatus
  ): Promise<ApiResponse<StudentStatus>> => {
    const response = await fetch(`${API_BASE_URL}/StudentStatus`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(status),
    });
    return parseApiResponse<StudentStatus>(response);
  },
  updateStudentStatus: async (
    status: StudentStatus
  ): Promise<ApiResponse<StudentStatus>> => {
    const response = await fetch(
      `${API_BASE_URL}/StudentStatus/${status.statusId}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(status),
      }
    );
    return parseApiResponse<StudentStatus>(response);
  },
  deleteStudentStatus: async (id: number): Promise<ApiResponse<string>> => {
    const response = await fetch(`${API_BASE_URL}/StudentStatus/${id}`, {
      method: "DELETE",
    });
    return parseApiResponse<string>(response);
  },
  getPrograms: async (): Promise<ApiResponse<program[]>> => {
    const response = await fetch(`${API_BASE_URL}/ApplicationProgram`);
    return parseApiResponse<program[]>(response);
  },
  addProgram: async (prog: program): Promise<ApiResponse<program>> => {
    const response = await fetch(`${API_BASE_URL}/ApplicationProgram`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(prog),
    });
    return parseApiResponse<program>(response);
  },
  updateProgram: async (prog: program): Promise<ApiResponse<program>> => {
    const response = await fetch(
      `${API_BASE_URL}/ApplicationProgram/${prog.programId}`,
      {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(prog),
      }
    );
    return parseApiResponse<program>(response);
  },
  deleteProgram: async (id: number): Promise<ApiResponse<string>> => {
    const response = await fetch(`${API_BASE_URL}/ApplicationProgram/${id}`, {
      method: "DELETE",
    });
    return parseApiResponse<string>(response);
  },
};

// api.ts
import {
  Faculty,
  Student,
  StudentStatus,
  StudentRequest,
  ApiError,
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
  const responseData = await response.json();

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
};

const handleApiError = async (response: Response): Promise<never> => {
  const errorText = await response.text();
  console.error("API Error Response:", errorText);
  console.error("Response status:", response.status);

  try {
    const errorData: ApiError = JSON.parse(errorText);
    const errorMessage = errorData.title;

    if (errorData.errors) {
      const fieldErrors = Object.entries(errorData.errors).reduce(
        (acc, [field, messages]) => {
          acc[field] = messages.join(", ");
          return acc;
        },
        {} as Record<string, string>
      );
      throw { message: errorMessage, fieldErrors };
    }

    throw new Error(errorMessage);
  } catch (e) {
    throw new Error(errorText || "An error occurred");
  }
};

export const api = {
  async getFaculties(): Promise<ApiResponse<Faculty[]>> {
    const response = await fetch(`${API_BASE_URL}/Faculty`);
    return parseApiResponse<Faculty[]>(response);
  },

  async getPrograms(): Promise<ApiResponse<program[]>> {
    const response = await fetch(`${API_BASE_URL}/ApplicationProgram`);
    return parseApiResponse<program[]>(response);
  },

  async getStudentStatuses(): Promise<ApiResponse<StudentStatus[]>> {
    const response = await fetch(`${API_BASE_URL}/StudentStatus`);
    return parseApiResponse<StudentStatus[]>(response);
  },

  async getStudents(): Promise<ApiResponse<Student[]>> {
    const response = await fetch(`${API_BASE_URL}/Students`);
    return parseApiResponse<Student[]>(response);
  },

  async addStudent(student: StudentRequest): Promise<ApiResponse<Student>> {
    const url = `${API_BASE_URL}/Students`;
    const response = await fetch(url, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(student),
    });
    return parseApiResponse<Student>(response);
  },

  async updateStudent(student: StudentRequest): Promise<ApiResponse<Student>> {
    const url = `${API_BASE_URL}/Students/${student.studentId}`;
    const response = await fetch(url, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(student),
    });

    if (response.status === 204) {
      // If no content is returned, create a success response manually.
      return {
        data: student as unknown as Student, // Optionally, you can choose to return the updated student data
        succeeded: true,
        errors: [],
        message: "Student updated successfully.",
      };
    }

    return parseApiResponse<Student>(response);
  },

  async deleteStudent(mssv: string): Promise<ApiResponse<void>> {
    const url = `${API_BASE_URL}/Students/${mssv}`;
    const response = await fetch(url, { method: "DELETE" });
    return parseApiResponse<void>(response);
  },

  async getStudentById(studentId: string): Promise<ApiResponse<Student>> {
    const url = `${API_BASE_URL}/Students/${studentId}`;
    const response = await fetch(url);
    return parseApiResponse<Student>(response);
  },
};

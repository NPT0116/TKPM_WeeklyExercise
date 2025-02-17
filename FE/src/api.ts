import {
  Faculty,
  Student,
  StudentStatus,
  StudentRequest,
  ApiError,
  program,
} from "./interface";

const API_BASE_URL = "http://localhost:5230/api";

const handleApiError = async (response: Response): Promise<never> => {
  const errorText = await response.text();
  console.error("API Error Response:", errorText);
  console.error("Response status:", response.status);

  try {
    const errorData: ApiError = JSON.parse(errorText);
    let errorMessage = errorData.title;

    if (errorData.errors) {
      const errorMessages = Object.values(errorData.errors).flat().join("\n");
      errorMessage = errorMessages;
    }

    throw new Error(errorMessage);
  } catch (e) {
    // If error response is not JSON
    throw new Error(errorText || "An error occurred");
  }
};

export const api = {
  async getFaculties(): Promise<Faculty[]> {
    const response = await fetch(`${API_BASE_URL}/Faculty`);
    if (!response.ok) throw new Error("Failed to fetch faculties");
    return response.json();
  },

  async getPrograms(): Promise<program[]> {
    const response = await fetch(`${API_BASE_URL}/ApplicationProgram`);
    if (!response.ok) throw new Error("Failed to fetch programs");
    return response.json();
  },

  async getStudentStatuses(): Promise<StudentStatus[]> {
    const response = await fetch(`${API_BASE_URL}/StudentStatus`);
    if (!response.ok) throw new Error("Failed to fetch student statuses");
    return response.json();
  },

  async getStudents(): Promise<Student[]> {
    const response = await fetch(`${API_BASE_URL}/Students`);
    if (!response.ok) throw new Error("Failed to fetch students");
    return response.json();
  },

  async addStudent(student: StudentRequest): Promise<Student> {
    const url = `${API_BASE_URL}/Students`;
    console.log("API URL:", url);
    console.log(
      "Sending student data to API:",
      JSON.stringify(student, null, 2)
    );

    const response = await fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(student),
    });

    if (!response.ok) {
      console.error("Failed to add student:", response.status);
      await handleApiError(response);
    }

    const result = await response.json();
    console.log("API Response:", JSON.stringify(result, null, 2));
    return result;
  },

  async updateStudent(student: StudentRequest): Promise<Student> {
    const url = `${API_BASE_URL}/Students/${student.studentId}`;
    console.log("API URL:", url);
    console.log(
      "Sending update data to API:",
      JSON.stringify(student, null, 2)
    );

    const response = await fetch(url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(student),
    });

    if (!response.ok) {
      await handleApiError(response);
    }

    // If status is 204 (No Content), return the submitted data with status info
    if (response.status === 204) {
      console.log("Success with no content, using submitted data");
      // Get the status info from the existing statuses
      const status = await this.getStudentStatuses().then((statuses) =>
        statuses.find((s) => s.statusId === student.statusId)
      );
      const program = await this.getPrograms().then((programs) =>
        programs.find((p) => p.programId === student.programId)
      );
      const faculty = await this.getFaculties().then((faculties) =>
        faculties.find((f) => f.facultyId === student.facultyId)
      );

      return {
        ...student,
        faculty: { id: student.facultyId, name: faculty?.name || "" },
        program: { id: student.programId, name: program?.name || "" },
        status: {
          statusId: student.statusId,
          name: status?.name || "",
        },
      };
    }

    const result = await response.json();
    console.log("API Response:", JSON.stringify(result, null, 2));
    return result;
  },

  async deleteStudent(mssv: string): Promise<void> {
    const url = `${API_BASE_URL}/Students/${mssv}`;
    console.log("API URL:", url);

    const response = await fetch(url, {
      method: "DELETE",
    });

    if (!response.ok) {
      const errorText = await response.text();
      console.error("API Error Response:", errorText);
      console.error("Response status:", response.status);
      throw new Error(`Failed to delete student: ${errorText}`);
    }
  },

  getStudentById: async (studentId: string): Promise<Student> => {
    const url = `${API_BASE_URL}/Students/${studentId}`;
    console.log("API URL:", url);

    const response = await fetch(url);
    if (!response.ok) {
      const errorText = await response.text();
      console.error("API Error Response:", errorText);
      console.error("Response status:", response.status);
      throw new Error("Student not found");
    }

    const result = await response.json();
    console.log("API Response:", JSON.stringify(result, null, 2));
    return result;
  },
};

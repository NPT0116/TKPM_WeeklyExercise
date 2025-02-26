// ... existing code ...

export interface Student {
  studentId: string;
  fullName: string;
  dateOfBirth: string;
  gender: number;
  facultyId: number;
  course: number;
  programId: number;
  address: string;
  email: string;
  phoneNumber: string;
  statusId: number;
  faculty?: {
    id: number;
    name: string;
  };
  program?: {
    id: number;
    name: string;
  };
  status?: {
    id: number;
    name: string;
  };
}

export interface StudentRequest {
  studentId: string;
  fullName: string;
  dateOfBirth: string;
  gender: number;
  facultyId: number;
  course: number;
  programId: number;
  address: string;
  email: string;
  phoneNumber: string;
  statusId: number;
}

export interface Faculty {
  facultyId: number;
  name: string;
}

export interface StudentStatus {
  statusId: number;
  name: string;
}

export interface program {
  programId: number;
  name: string;
}

export interface ApiError {
  type: string;
  title: string;
  status: number;
  errors?: {
    [key: string]: string[];
  };
  traceId: string;
}

export interface ApiErrorResponse {
  error: ApiError;
  message: string;
}

// ... existing code ...

import React from "react";
import StudentSearchPanel from "../components/StudentSearchPanel";
import StudentListScreen from "../screens/StudentListScreen";
import ImportExportPanel from "../components/ImportExportPanel";
import StudentForm from "../components/StudentForm";
import {
  Student,
  Faculty,
  StudentStatus,
  StudentRequest,
  program,
} from "../interface";
import { ApiResponse } from "../api";

interface StudentSectionProps {
  students: Student[];
  faculties: Faculty[];
  statuses: StudentStatus[];
  programs: program[];
  selectedStudent: Student | null;
  isFormOpen: boolean;
  error: React.ReactNode | null;
  // This function is used when the form is submitted (API call)
  onAddStudent: (student: StudentRequest) => Promise<ApiResponse<Student>>;
  onUpdateStudent: (student: StudentRequest) => Promise<ApiResponse<Student>>;
  onDeleteStudent: (studentId: string) => void;
  onCloseForm: () => void;
  onEditStudent: (student: Student) => void;
  // New callback just to open the form for adding a new student
  onOpenAddForm: () => void;
  onSearch: (params: {
    mssv?: string;
    facultyId?: number | null;
    name?: string;
  }) => void;
  onImportSuccess?: () => void;
}

const StudentSection: React.FC<StudentSectionProps> = ({
  students,
  faculties,
  statuses,
  programs,
  selectedStudent,
  isFormOpen,
  error,
  onAddStudent,
  onUpdateStudent,
  onDeleteStudent,
  onCloseForm,
  onEditStudent,
  onOpenAddForm,
  onSearch,
  onImportSuccess,
}) => {
  return (
    <>
      {!isFormOpen ? (
        <>
          <StudentSearchPanel faculties={faculties} onSearch={onSearch} />
          <StudentListScreen
            students={students}
            onAddNew={onOpenAddForm} // Use the callback to open the form
            onEdit={onEditStudent}
            onDelete={onDeleteStudent}
            onSearch={() => {}}
          />
          <ImportExportPanel onImportSuccess={onImportSuccess} />
        </>
      ) : (
        <StudentForm
          student={selectedStudent}
          faculties={faculties}
          statuses={statuses}
          programs={programs}
          error={error}
          onSubmit={selectedStudent ? onUpdateStudent : onAddStudent}
          onClose={onCloseForm}
        />
      )}
    </>
  );
};

export default StudentSection;

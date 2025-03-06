// src/components/StudentStatusSection.tsx
import React from "react";
import { StudentStatus } from "../interface";
import StudentStatusListScreen from "../screens/StudentStatusListScreen";
import StudentStatusForm from "./StudentStatusForm";

interface StudentStatusSectionProps {
  statuses: StudentStatus[];
  selectedStatus: StudentStatus | null;
  isFormOpen: boolean;
  error: string | React.ReactNode | null;
  onAddStatus: (status: StudentStatus) => void;
  onUpdateStatus: (status: StudentStatus) => void;
  onDeleteStatus: (statusId: number) => void;
  onCloseForm: () => void;
  onEditStatus: (status: StudentStatus) => void;
}

const StudentStatusSection: React.FC<StudentStatusSectionProps> = ({
  statuses,
  selectedStatus,
  isFormOpen,
  error,
  onAddStatus,
  onUpdateStatus,
  onDeleteStatus,
  onCloseForm,
  onEditStatus,
}) => {
  return (
    <div className="status-section">
      {!isFormOpen ? (
        <StudentStatusListScreen
          statuses={statuses}
          onAddNew={onCloseForm}
          onEdit={onEditStatus}
          onDelete={onDeleteStatus}
        />
      ) : (
        <StudentStatusForm
          status={selectedStatus}
          onSubmit={selectedStatus ? onUpdateStatus : onAddStatus}
          onClose={onCloseForm}
        />
      )}
    </div>
  );
};

export default StudentStatusSection;

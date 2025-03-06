// src/components/FacultySection.tsx
import React from "react";
import { Faculty } from "../interface";
import FacultyListScreen from "../screens/FacultyListScreen";
import FacultyForm from "./FacultyForm";

interface FacultySectionProps {
  faculties: Faculty[];
  selectedFaculty: Faculty | null;
  isFormOpen: boolean;
  error: string | React.ReactNode | null;
  onAddFaculty: (faculty: Faculty) => void;
  onUpdateFaculty: (faculty: Faculty) => void;
  onDeleteFaculty: (facultyId: number) => void;
  onCloseForm: () => void;
  onEditFaculty: (faculty: Faculty) => void;
}

const FacultySection: React.FC<FacultySectionProps> = ({
  faculties,
  selectedFaculty,
  isFormOpen,
  error,
  onAddFaculty,
  onUpdateFaculty,
  onDeleteFaculty,
  onCloseForm,
  onEditFaculty,
}) => {
  return (
    <div className="faculty-section">
      {!isFormOpen ? (
        <FacultyListScreen
          faculties={faculties}
          onAddNew={onCloseForm}
          onEdit={onEditFaculty}
          onDelete={onDeleteFaculty}
        />
      ) : (
        <FacultyForm
          faculty={selectedFaculty}
          onSubmit={selectedFaculty ? onUpdateFaculty : onAddFaculty}
          onClose={onCloseForm}
        />
      )}
    </div>
  );
};

export default FacultySection;

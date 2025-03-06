// src/components/ProgramSection.tsx
import React from "react";
import { program } from "../interface";
import ProgramListScreen from "../screens/ProgramListScreen";
import ProgramForm from "./ProgramForm";

interface ProgramSectionProps {
  programs: program[];
  selectedProgram: program | null;
  isFormOpen: boolean;
  error: string | React.ReactNode | null;
  onAddProgram: (prog: program) => void;
  onUpdateProgram: (prog: program) => void;
  onDeleteProgram: (programId: number) => void;
  onCloseForm: () => void;
  onEditProgram: (prog: program) => void;
}

const ProgramSection: React.FC<ProgramSectionProps> = ({
  programs,
  selectedProgram,
  isFormOpen,
  error,
  onAddProgram,
  onUpdateProgram,
  onDeleteProgram,
  onCloseForm,
  onEditProgram,
}) => {
  return (
    <div className="program-section">
      {!isFormOpen ? (
        <ProgramListScreen
          programs={programs}
          onAddNew={onCloseForm}
          onEdit={onEditProgram}
          onDelete={onDeleteProgram}
          error={error}
        />
      ) : (
        <ProgramForm
          program={selectedProgram}
          onSubmit={selectedProgram ? onUpdateProgram : onAddProgram}
          onClose={onCloseForm}
        />
      )}
    </div>
  );
};

export default ProgramSection;

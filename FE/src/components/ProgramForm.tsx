// src/components/ProgramForm.tsx
import React, { useState, useEffect } from "react";
import { program } from "../interface";

interface ProgramFormProps {
  program: program | null;
  onSubmit: (prog: program) => void;
  onClose: () => void;
}

const ProgramForm: React.FC<ProgramFormProps> = ({
  program,
  onSubmit,
  onClose,
}) => {
  const [name, setName] = useState(program ? program.name : "");

  useEffect(() => {
    if (program) {
      setName(program.name);
    }
  }, [program]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const updatedProgram: program = {
      programId: program ? program.programId : 0,
      name,
    };
    onSubmit(updatedProgram);
  };

  return (
    <div>
      <h2>{program ? "Sửa Chương trình" : "Thêm Chương trình"}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Tên Chương trình:</label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div>
          <button type="submit">{program ? "Cập nhật" : "Thêm"}</button>
          <button type="button" onClick={onClose}>
            Hủy
          </button>
        </div>
      </form>
    </div>
  );
};

export default ProgramForm;

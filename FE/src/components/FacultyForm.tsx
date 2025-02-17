// src/components/FacultyForm.tsx
import React, { useState, useEffect } from "react";
import { Faculty } from "../interface";

interface FacultyFormProps {
  faculty: Faculty | null;
  onSubmit: (faculty: Faculty) => void;
  onClose: () => void;
}

const FacultyForm: React.FC<FacultyFormProps> = ({
  faculty,
  onSubmit,
  onClose,
}) => {
  const [name, setName] = useState(faculty ? faculty.name : "");

  useEffect(() => {
    if (faculty) {
      setName(faculty.name);
    }
  }, [faculty]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Với trường hợp thêm mới, facultyId có thể là 0 (backend sẽ tạo mới)
    const updatedFaculty: Faculty = {
      facultyId: faculty ? faculty.facultyId : 0,
      name,
    };
    onSubmit(updatedFaculty);
  };

  return (
    <div>
      <h2>{faculty ? "Sửa Khoa" : "Thêm Khoa"}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Tên Khoa:</label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div>
          <button type="submit">{faculty ? "Cập nhật" : "Thêm"}</button>
          <button type="button" onClick={onClose}>
            Hủy
          </button>
        </div>
      </form>
    </div>
  );
};

export default FacultyForm;

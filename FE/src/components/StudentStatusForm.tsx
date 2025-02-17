// src/components/StudentStatusForm.tsx
import React, { useState, useEffect } from "react";
import { StudentStatus } from "../interface";

interface StudentStatusFormProps {
  status: StudentStatus | null;
  onSubmit: (status: StudentStatus) => void;
  onClose: () => void;
}

const StudentStatusForm: React.FC<StudentStatusFormProps> = ({
  status,
  onSubmit,
  onClose,
}) => {
  const [name, setName] = useState(status ? status.name : "");

  useEffect(() => {
    if (status) {
      setName(status.name);
    }
  }, [status]);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const updatedStatus: StudentStatus = {
      statusId: status ? status.statusId : 0,
      name,
    };
    onSubmit(updatedStatus);
  };

  return (
    <div>
      <h2>{status ? "Sửa Tình trạng" : "Thêm Tình trạng"}</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Tên Tình trạng:</label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div>
          <button type="submit">{status ? "Cập nhật" : "Thêm"}</button>
          <button type="button" onClick={onClose}>
            Hủy
          </button>
        </div>
      </form>
    </div>
  );
};

export default StudentStatusForm;

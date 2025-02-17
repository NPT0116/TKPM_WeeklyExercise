// src/screens/FacultyListScreen.tsx
import React from "react";
import { Faculty } from "../interface";

interface FacultyListScreenProps {
  faculties: Faculty[];
  onAddNew: () => void;
  onEdit: (faculty: Faculty) => void;
  onDelete: (facultyId: number) => void;
}

const FacultyListScreen: React.FC<FacultyListScreenProps> = ({
  faculties,
  onAddNew,
  onEdit,
  onDelete,
}) => {
  return (
    <div>
      <h2>Quản lý Khoa</h2>
      <button onClick={onAddNew}>Thêm Khoa mới</button>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Tên</th>
            <th>Hành động</th>
          </tr>
        </thead>
        <tbody>
          {faculties.map((faculty) => (
            <tr key={faculty.facultyId}>
              <td>{faculty.facultyId}</td>
              <td>{faculty.name}</td>
              <td>
                <button onClick={() => onEdit(faculty)}>Sửa</button>
                <button onClick={() => onDelete(faculty.facultyId)}>Xóa</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default FacultyListScreen;

// src/screens/StudentStatusListScreen.tsx
import React from "react";
import { StudentStatus } from "../interface";

interface StudentStatusListScreenProps {
  statuses: StudentStatus[];
  onAddNew: () => void;
  onEdit: (status: StudentStatus) => void;
  onDelete: (statusId: number) => void;
}

const StudentStatusListScreen: React.FC<StudentStatusListScreenProps> = ({
  statuses,
  onAddNew,
  onEdit,
  onDelete,
}) => {
  return (
    <div>
      <h2>Quản lý Tình trạng sinh viên</h2>
      <button onClick={onAddNew}>Thêm Tình trạng mới</button>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Tên</th>
            <th>Hành động</th>
          </tr>
        </thead>
        <tbody>
          {statuses.map((status) => (
            <tr key={status.statusId}>
              <td>{status.statusId}</td>
              <td>{status.name}</td>
              <td>
                <button onClick={() => onEdit(status)}>Sửa</button>
                <button onClick={() => onDelete(status.statusId)}>Xóa</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default StudentStatusListScreen;

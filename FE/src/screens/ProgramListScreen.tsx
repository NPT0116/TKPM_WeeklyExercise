// src/screens/ProgramListScreen.tsx
import React from "react";
import { program } from "../interface";

interface ProgramListScreenProps {
  programs: program[];
  onAddNew: () => void;
  onEdit: (prog: program) => void;
  onDelete: (programId: number) => void;
}

const ProgramListScreen: React.FC<ProgramListScreenProps> = ({
  programs,
  onAddNew,
  onEdit,
  onDelete,
}) => {
  return (
    <div>
      <h2>Quản lý Chương trình đào tạo</h2>
      <button onClick={onAddNew}>Thêm Chương trình mới</button>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Tên</th>
            <th>Hành động</th>
          </tr>
        </thead>
        <tbody>
          {programs.map((prog) => (
            <tr key={prog.programId}>
              <td>{prog.programId}</td>
              <td>{prog.name}</td>
              <td>
                <button onClick={() => onEdit(prog)}>Sửa</button>
                <button onClick={() => onDelete(prog.programId)}>Xóa</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ProgramListScreen;

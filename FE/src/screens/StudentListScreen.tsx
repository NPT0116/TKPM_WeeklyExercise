// src/screens/StudentListScreen.tsx
import React from "react";
import { Student } from "../interface";
import StudentList from "../components/StudentList";

interface StudentListScreenProps {
  students: Student[];
  onAddNew: () => void;
  onDelete: (mssv: string) => void;
  onEdit: (student: Student) => void;
  onSearch: (query: string) => void;
}

const StudentListScreen: React.FC<StudentListScreenProps> = ({
  students,
  onAddNew,
  onDelete,
  onEdit,
  onSearch,
}) => {
  return (
    <div className="screen student-list-screen">
      <header className="screen-header">
        <h1>Quản Lý Sinh Viên</h1>
        <button onClick={onAddNew}>Thêm Sinh Viên Mới</button>
      </header>

      {/* Optionally include a search bar */}
      {/* <SearchBar onSearch={onSearch} /> */}

      <StudentList students={students} onDelete={onDelete} onEdit={onEdit} />
    </div>
  );
};

export default StudentListScreen;

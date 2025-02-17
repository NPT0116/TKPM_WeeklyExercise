import React from "react";
import { Student } from "../interface";

interface StudentListProps {
  students: Student[];
  onDelete: (mssv: string) => void;
  onEdit: (student: Student) => void;
}

const StudentList: React.FC<StudentListProps> = ({
  students,
  onDelete,
  onEdit,
}) => {
  const getGenderText = (gender: number) => (gender === 0 ? "Nam" : "Nữ");

  return (
    <div className="student-list">
      <table>
        <thead>
          <tr>
            <th>MSSV</th>
            <th>Họ và tên</th>
            <th>Ngày sinh</th>
            <th>Giới tính</th>
            <th>Khoa</th>
            <th>Khóa</th>
            <th>Chương trình</th>
            <th>Địa chỉ</th>
            <th>Email</th>
            <th>SĐT</th>
            <th>Tình trạng</th>
            <th>Thao tác</th>
          </tr>
        </thead>
        <tbody>
          {Array.isArray(students) && students.length > 0 ? (
            students.map((student) => (
              <tr key={student.studentId}>
                <td title={student.studentId}>{student.studentId}</td>
                <td title={student.fullName}>{student.fullName}</td>
                <td title={student.dateOfBirth}>
                  {new Date(student.dateOfBirth).toLocaleDateString()}
                </td>
                <td>{getGenderText(student.gender)}</td>
                <td title={student.program?.name || ""}>
                  {student.program?.name || "N/A"}
                </td>
                <td>{student.course}</td>
                <td title={student.faculty?.name || ""}>
                  {student.faculty?.name || "N/A"}
                </td>
                <td title={student.address}>{student.address}</td>
                <td title={student.email}>{student.email}</td>
                <td title={student.phoneNumber}>{student.phoneNumber}</td>
                <td title={student.status?.name || ""}>
                  {student.status?.name || "N/A"}
                </td>
                <td>
                  <button onClick={() => onEdit(student)}>Sửa</button>
                  <button onClick={() => onDelete(student.studentId)}>
                    Xóa
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={12}>Không có sinh viên nào</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default StudentList;

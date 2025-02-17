// src/components/StudentSearchPanel.tsx
import React, { useState } from "react";
import { Faculty } from "../interface";

export type SearchType = "mssv" | "faculty" | "facultyName";

interface StudentSearchPanelProps {
  faculties: Faculty[];
  onSearch: (params: {
    mssv?: string;
    facultyId?: number | null;
    name?: string;
  }) => void;
}

const StudentSearchPanel: React.FC<StudentSearchPanelProps> = ({
  faculties,
  onSearch,
}) => {
  const [searchType, setSearchType] = useState<SearchType>("mssv");
  const [mssv, setMssv] = useState("");
  const [facultyId, setFacultyId] = useState<number | null>(null);
  const [name, setName] = useState("");

  const handleSearchClick = () => {
    if (searchType === "mssv") {
      onSearch({ mssv });
    } else if (searchType === "faculty") {
      onSearch({ facultyId });
    } else if (searchType === "facultyName") {
      onSearch({ facultyId, name });
    }
  };

  return (
    <div className="student-search-panel">
      <div>
        <label>
          <input
            type="radio"
            value="mssv"
            checked={searchType === "mssv"}
            onChange={() => setSearchType("mssv")}
          />
          Tìm theo MSSV
        </label>
        <label>
          <input
            type="radio"
            value="faculty"
            checked={searchType === "faculty"}
            onChange={() => setSearchType("faculty")}
          />
          Tìm theo Khoa
        </label>
        <label>
          <input
            type="radio"
            value="facultyName"
            checked={searchType === "facultyName"}
            onChange={() => setSearchType("facultyName")}
          />
          Tìm theo Khoa + Tên sinh viên
        </label>
      </div>

      <div>
        {searchType === "mssv" && (
          <>
            <input
              type="text"
              placeholder="Nhập MSSV"
              value={mssv}
              onChange={(e) => setMssv(e.target.value)}
            />
          </>
        )}
        {(searchType === "faculty" || searchType === "facultyName") && (
          <>
            <select
              onChange={(e) =>
                setFacultyId(e.target.value ? Number(e.target.value) : null)
              }
            >
              <option value="">-- Chọn khoa --</option>
              {faculties.map((f) => (
                <option key={f.facultyId} value={f.facultyId}>
                  {f.name}
                </option>
              ))}
            </select>
            {searchType === "facultyName" && (
              <input
                type="text"
                placeholder="Nhập tên sinh viên"
                value={name}
                onChange={(e) => setName(e.target.value)}
              />
            )}
          </>
        )}
      </div>
      <button onClick={handleSearchClick}>Tìm kiếm</button>
    </div>
  );
};

export default StudentSearchPanel;

// src/components/NavigationBar.tsx
import React from "react";

export type Tab =
  | "students"
  | "faculties"
  | "statuses"
  | "programs"
  | "certificate"
  | "business";

interface NavigationBarProps {
  currentTab: Tab;
  onChangeTab: (tab: Tab) => void;
}

const NavigationBar: React.FC<NavigationBarProps> = ({
  currentTab,
  onChangeTab,
}) => {
  return (
    <nav className="navigation-bar">
      <button
        className={currentTab === "students" ? "active" : ""}
        onClick={() => onChangeTab("students")}
      >
        Sinh viên
      </button>
      <button
        className={currentTab === "faculties" ? "active" : ""}
        onClick={() => onChangeTab("faculties")}
      >
        Khoa
      </button>
      <button
        className={currentTab === "statuses" ? "active" : ""}
        onClick={() => onChangeTab("statuses")}
      >
        Tình trạng
      </button>
      <button
        className={currentTab === "programs" ? "active" : ""}
        onClick={() => onChangeTab("programs")}
      >
        Chương trình
      </button>
      <button
        className={currentTab === "certificate" ? "active" : ""}
        onClick={() => onChangeTab("certificate")}
      >
        Giấy xác nhận
      </button>
      <button
        className={currentTab === "business" ? "active" : ""}
        onClick={() => onChangeTab("business")}
      >
        Điều chỉnh quy định
      </button>
    </nav>
  );
};

export default NavigationBar;

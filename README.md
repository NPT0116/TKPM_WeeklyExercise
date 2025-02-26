# Ex03 – Design Changes and Testing Improvements

This document outlines the design changes introduced in Ex03 to support critical business logic requirements and to improve testability. In the new design, a dedicated service layer is introduced between the controller and repository. This service layer encapsulates all business logic, making it easier to unit test and maintain.

---

## Business Logic Requirements

1. **Unique Student ID (MSSV)**

   - When adding or updating a student, the Student ID must be unique. The system ensures that no two students share the same MSSV.

2. **Configurable Email Domain**

   - Email addresses must belong to a specific domain. For example, only emails ending with `@student.university.edu.vn` are accepted.
   - The allowed domain is configurable via application settings.

3. **Configurable Phone Number Format**

   - Phone numbers must follow a valid format for a given country. For Vietnam, acceptable formats are either starting with `+84` or `0[3|5|7|8|9]` followed by 8 digits.
   - This phone number format is configurable through application settings.

4. **Student Status Transition Rules**
   - Student status can only change according to specific business rules:
     - For example, a student with status `"Đang học"` can transition to `"Bảo lưu"`, `"Tốt nghiệp"`, or `"Đình chỉ"`.
     - A student with status `"Đã tốt nghiệp"` is not allowed to revert back to `"Đang học"`.
   - These transitions are configurable via application settings.

---

## Design Solution

### Introducing a Service Layer

The original design had the controller directly interacting with the repository and handling all business logic. This approach made the controller tightly coupled with multiple dependencies, which in turn complicated unit testing.

**New Design Highlights:**

- **Dedicated Student Service:**  
  A new `StudentService` is implemented to encapsulate all business logic. This service handles:
  - Validation of unique Student IDs.
  - Verification of email domain and phone number format using configurable settings.
  - Enforcement of valid status transitions according to the business rules.
- **Controller Simplification:**  
  The controller now only delegates requests to the `StudentService`, which isolates the business logic from the web layer. This separation makes the controller easier to test and maintain.
- **Mocking Dependencies for Testing:**  
  With the business logic isolated in the service layer, it becomes much easier to use mocks (e.g., Moq) to simulate dependencies (like repository, configuration, and validators) in unit tests. This improves the overall testability of the application.

---

## 3. Evaluation and Design Improvements

### **Issues Identified in Previous Design:**

- **Heavy Dependency on Persistent Storage:**  
  The original design tightly coupled the controller with the database via the repository, making unit testing difficult.  
  **Improvement:** Use a service layer to encapsulate business logic and rely on mocks/stubs for unit tests.

- **Overloaded Modules:**  
  The controller handled too many responsibilities (request handling and business logic).  
  **Improvement:** Refactor into separate layers (controller for I/O, service for business logic) to follow the Single Responsibility Principle.

- **Limited Testability of Critical Business Logic:**  
  Key rules such as unique MSSV, email domain verification, phone format validation, and status transitions were hard to test.  
  **Improvement:** Isolate these business rules within a dedicated service layer, making it easier to write comprehensive unit tests.

### **Reporting and Continuous Improvement:**

- **Documentation:**  
  This README describes the design changes and testing strategy to help team members understand the rationale behind the new structure.
- **Test Reports:**  
  In case of testing challenges, a brief report should be provided detailing the issues encountered and suggestions for further refactoring.
- **Code Refactoring:**  
  Any further improvements, such as splitting services into even finer-grained components or introducing additional configuration options, will be documented and discussed with the team.

---

## Summary

- **New Service Layer:**  
  The introduction of a dedicated `StudentService` decouples business logic from the controller and repository. This makes our codebase easier to test and maintain.
- **Configurable Business Rules:**  
  Key validations such as unique MSSV, email domain, phone number format, and student status transitions are now configurable via settings, increasing flexibility.
- **Improved Testability:**  
  With business logic isolated in the service layer, unit tests can be written using mocks for all external dependencies, leading to more reliable and maintainable tests.

This design change not only simplifies our application architecture but also significantly enhances our ability to perform thorough unit and integration testing.

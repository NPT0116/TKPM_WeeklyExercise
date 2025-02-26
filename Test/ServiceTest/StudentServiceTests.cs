using System;
using BE.Dto;
using BE.Exceptions.Student;
using BE.Exceptions.StudentStatus;
using BE.Interface;
using BE.Models;
using BE.Services;
using Moq;

namespace test.ServiceTest;


 public class StudentServiceTests
    {
        private readonly Mock<IStudentRepository> _studentRepoMock;
        private readonly Mock<IValidateStudentEmail> _validateEmailMock;
        private readonly Mock<IValidateStudentPhone> _validatePhoneMock;
        private readonly Mock<IFacultyRepository> _facultyRepoMock;
        private readonly Mock<IApplicationProgramRepository> _programRepoMock;
        private readonly Mock<IStudentStatusRepository> _statusRepoMock;
        private readonly Mock<IStudentStatusTransitionService> _statusTransitionMock;
        private readonly StudentService _studentService;

        public StudentServiceTests()
        {
            _studentRepoMock = new Mock<IStudentRepository>();
            _validateEmailMock = new Mock<IValidateStudentEmail>();
            _validatePhoneMock = new Mock<IValidateStudentPhone>();
            _facultyRepoMock = new Mock<IFacultyRepository>();
            _programRepoMock = new Mock<IApplicationProgramRepository>();
            _statusRepoMock = new Mock<IStudentStatusRepository>();
            _statusTransitionMock = new Mock<IStudentStatusTransitionService>();

            _studentService = new StudentService(
                _studentRepoMock.Object,
                _validateEmailMock.Object,
                _validatePhoneMock.Object,
                _facultyRepoMock.Object,
                _programRepoMock.Object,
                _statusRepoMock.Object,
                _statusTransitionMock.Object);
        }

        [Fact]
        public async Task AddStudentServiceAsync_ValidStudent_ReturnsStudent()
        {
            // Arrange
            var studentDto = new StudentCreateDto
            {
                StudentId = "22127333",
                FullName = "Nguyễn Văn A",
                DateOfBirth = DateTime.UtcNow,
                Gender = 0,
                FacultyId = 1,
                Course = 2022,
                ProgramId = 1,
                Address = "123 Some Street",
                Email = "test@student.university.edu.vn",
                PhoneNumber = "0912345678",
                StatusId = 1
            };

            _validateEmailMock.Setup(x => x.IsValidEmail(studentDto.Email)).Returns(true);
            _validatePhoneMock.Setup(x => x.IsValidPhone(studentDto.PhoneNumber)).Returns(true);
            _facultyRepoMock.Setup(x => x.GetByIdAsync(studentDto.FacultyId))
                .ReturnsAsync(new Faculty { FacultyId = 1, Name = "Khoa Luật" });
            _programRepoMock.Setup(x => x.GetByIdAsync(studentDto.ProgramId))
                .ReturnsAsync(new ApplicationProgram { ProgramId = 1, Name = "Chất lượng cao" });
            _statusRepoMock.Setup(x => x.GetByIdAsync(studentDto.StatusId))
                .ReturnsAsync(new StudentStatus { StatusId = 1, Name = "Đang học" });

            _studentRepoMock.Setup(x => x.CreateAsync(studentDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _studentService.AddStudentServiceAsync(studentDto);

            // Assert
            Assert.Equal(studentDto, result);
            _studentRepoMock.Verify(x => x.CreateAsync(studentDto), Times.Once);
        }

        [Fact]
        public async Task AddStudentServiceAsync_InvalidEmail_ThrowsStudentEmailFormatError()
        {
            // Arrange
            var studentDto = new StudentCreateDto
            {
                StudentId = "22127333",
                Email = "invalidemail",
                PhoneNumber = "0912345678",
                FacultyId = 1,
                ProgramId = 1,
                StatusId = 1
            };

            _validateEmailMock.Setup(x => x.IsValidEmail(studentDto.Email)).Returns(false);
            _validateEmailMock.Setup(x => x.GetAllowedDomain()).Returns("@student.university.edu.vn");

            // Act & Assert
            await Assert.ThrowsAsync<StudentEmailFormatError>(() => _studentService.AddStudentServiceAsync(studentDto));
        }

        [Fact]
        public async Task UpdateStudentServiceAsync_ValidUpdate_ReturnsUpdatedStudent()
        {
            // Arrange
            var studentUpdateDto = new StudentUpdateDto
            {
                StudentId = "22127333",
                FullName = "Nguyễn Văn A Updated",
                DateOfBirth = DateTime.UtcNow,
                Gender = 0,
                FacultyId = 1,
                Course = 2022,
                ProgramId = 1,
                Address = "123 Some Street",
                Email = "test@student.university.edu.vn",
                PhoneNumber = "0912345678",
                StatusId = 2  // new status
            };

            var existingStudent = new Student
            {
                StudentId = "22127333",
                FullName = "Nguyễn Văn A",
                DateOfBirth = DateTime.UtcNow,
                Gender = 0,
                Faculty = new Faculty { FacultyId = 1, Name = "Khoa Luật" },
                Course = 2022,
                Program = new ApplicationProgram { ProgramId = 1, Name = "Chất lượng cao" },
                Address = "123 Some Street",
                Email = "test@student.university.edu.vn",
                PhoneNumber = "0912345678",
                Status = new StudentStatus { StatusId = 1, Name = "Đang học" }
            };

            _validateEmailMock.Setup(x => x.IsValidEmail(studentUpdateDto.Email)).Returns(true);
            _validatePhoneMock.Setup(x => x.IsValidPhone(studentUpdateDto.PhoneNumber)).Returns(true);
            _studentRepoMock.Setup(x => x.GetByIdAsync(studentUpdateDto.StudentId))
                .ReturnsAsync(
                    new StudentDto{
                        StudentId = "22127333",
                        FullName = "Nguyễn Văn A",
                        DateOfBirth = DateTime.UtcNow,
                        Gender = 0,
                        Faculty = new facultyDto (1,  "Khoa Luật" ),
                        Course = 2022,
                        Program = new applicationProgramDto (1, "Chất lượng cao" ),
                        Address = "123 Some Street",
                        Email = "test@student.university.edu.vn",
                        PhoneNumber = "0912345678",
                        Status = new studentStatusDto ( 1, "Đang học" )
                    }
                );
            _facultyRepoMock.Setup(x => x.GetByIdAsync(studentUpdateDto.FacultyId))
                .ReturnsAsync(new Faculty { FacultyId = 1, Name = "Khoa Luật" });
            _programRepoMock.Setup(x => x.GetByIdAsync(studentUpdateDto.ProgramId))
                .ReturnsAsync(new ApplicationProgram { ProgramId = 1, Name = "Chất lượng cao" });
            _statusRepoMock.Setup(x => x.GetByIdAsync(studentUpdateDto.StatusId))
                .ReturnsAsync(new StudentStatus { StatusId = studentUpdateDto.StatusId, Name = "Bảo lưu" });
            _statusTransitionMock.Setup(x => x.IsValidTransition(existingStudent.Status.StatusId, studentUpdateDto.StatusId))
                .Returns(true);
            _studentRepoMock.Setup(x => x.UpdateAsync(studentUpdateDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _studentService.UpdateStudentServiceAsync(studentUpdateDto);

            // Assert
            Assert.Equal(studentUpdateDto, result);
            _studentRepoMock.Verify(x => x.UpdateAsync(studentUpdateDto), Times.Once);
        }

        [Fact]
        public async Task UpdateStudentServiceAsync_InvalidStatusTransition_ThrowsStudentStatusTransisentError()
        {
            // Arrange
            var studentUpdateDto = new StudentUpdateDto
            {
                StudentId = "22127333",
                Email = "test@student.university.edu.vn",
                PhoneNumber = "0912345678",
                FacultyId = 1,
                ProgramId = 1,
                StatusId = 2  // new status
            };

            var existingStudent = new Student
            {
                StudentId = "22127333",
                Status = new StudentStatus { StatusId = 1, Name = "Đang học" }
            };

            _validateEmailMock.Setup(x => x.IsValidEmail(studentUpdateDto.Email)).Returns(true);
            _validatePhoneMock.Setup(x => x.IsValidPhone(studentUpdateDto.PhoneNumber)).Returns(true);
            _studentRepoMock.Setup(x => x.GetByIdAsync(studentUpdateDto.StudentId))
                .ReturnsAsync(
                    new StudentDto{
                        StudentId = "22127333",
                        Status = new studentStatusDto ( 1, "Đang học" )
                    }
                );
            _facultyRepoMock.Setup(x => x.GetByIdAsync(studentUpdateDto.FacultyId))
                .ReturnsAsync(new Faculty { FacultyId = 1, Name = "Khoa Luật" });
            _programRepoMock.Setup(x => x.GetByIdAsync(studentUpdateDto.ProgramId))
                .ReturnsAsync(new ApplicationProgram { ProgramId = 1, Name = "Chất lượng cao" });
            _statusRepoMock.Setup(x => x.GetByIdAsync(studentUpdateDto.StatusId))
                .ReturnsAsync(new StudentStatus { StatusId = studentUpdateDto.StatusId, Name = "Bảo lưu" });
            _statusTransitionMock.Setup(x => x.IsValidTransition(existingStudent.Status.StatusId, studentUpdateDto.StatusId))
                .Returns(false);

            // Act & Assert
            await Assert.ThrowsAsync<StudentStatusTransisentError>(() => _studentService.UpdateStudentServiceAsync(studentUpdateDto));
        }
}

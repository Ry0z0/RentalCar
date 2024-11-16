import { Component, OnInit } from '@angular/core';
import { AdminService, IUser } from '../../../core/services/Admin/admin.service';  // Đảm bảo đường dẫn đúng

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  users: IUser[] = [];  // Mảng để lưu danh sách người dùng
  paginatedUsers: IUser[] = [];  // Danh sách người dùng được hiển thị trên trang hiện tại

  currentPage: number = 1;
  pageSize: number = 15;
  totalPages: number = 1;
  userType: string = 'all';  // Biến để theo dõi loại người dùng hiện tại ('all', 'customer', 'carowner')
  
  sortColumn: keyof IUser | '' = '';  // Cột đang được sắp xếp
  sortDirection: 'asc' | 'desc' | '' = '';  // Hướng sắp xếp ('asc' hoặc 'desc')

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.loadAllUsers();  // Gọi hàm để tải danh sách người dùng khi component khởi tạo
  }

  loadAllUsers(): void {
    this.adminService.getAllUsers().subscribe(
      (data: any) => {
        this.users = data.users as IUser[];  // Chuyển đổi dữ liệu nhận được thành mảng IUser
        this.totalPages = Math.ceil(this.users.length / this.pageSize);
        this.updatePaginatedUsers();
        this.userType = 'all';
      },
      error => {
        console.error('Error fetching user data', error);
      }
    );
  }

  loadUsersByType(type: string): void {
    this.adminService.getUsersByType(type).subscribe(
      (data: any) => {
        this.users = data.users as IUser[];
        this.totalPages = Math.ceil(this.users.length / this.pageSize);
        this.updatePaginatedUsers();
        this.userType = type;
      },
      error => {
        console.error(`Error fetching ${type} data`, error);
      }
    );
  }

  deleteUser(id: string): void {
    if (confirm('Are you sure you want to delete this user?')) {  // Xác nhận trước khi xóa
      this.adminService.deleteUser(id).subscribe(
        response => {
          console.log(response.message);  // Xử lý phản hồi từ API nếu cần
          this.loadAllUsers();  // Tải lại danh sách người dùng sau khi xóa thành công
        },
        error => {
          console.error('Error deleting user', error);
        }
      );
    }
  }
  onSearch(event: Event): void {
    const input = event.target as HTMLInputElement;
    const query = input.value;
    
    if (query) {
      this.adminService.searchUsers(query).subscribe(
        (data: any) => {
          this.users = data.users as IUser[];
          this.totalPages = Math.ceil(this.users.length / this.pageSize);
          this.updatePaginatedUsers();
        },
        error => {
          console.error('Error searching users', error);
        }
      );
    } else {
      this.loadAllUsers();
    }
  }
  
  updatePaginatedUsers(): void {
    let sortedUsers = [...this.users];
    if (this.sortColumn) {
        sortedUsers.sort((a, b) => {
            const aValue = a[this.sortColumn as keyof IUser];
            const bValue = b[this.sortColumn as keyof IUser];

            if (aValue === undefined || bValue === undefined) return 0; // Thêm kiểm tra undefined

            if (aValue < bValue) return this.sortDirection === 'asc' ? -1 : 1;
            if (aValue > bValue) return this.sortDirection === 'asc' ? 1 : -1;
            return 0;
        });
    }
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.paginatedUsers = sortedUsers.slice(start, end);
}


  sort(column: keyof IUser): void {
    if (this.sortColumn === column) {
        this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
        this.sortColumn = column;
        this.sortDirection = 'asc';
    }
    this.updatePaginatedUsers();
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.updatePaginatedUsers();
  }
}

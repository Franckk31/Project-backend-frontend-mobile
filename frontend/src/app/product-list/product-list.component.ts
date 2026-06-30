import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../services/product.service';
import { Product } from '../models/product.model';
import { Users } from '../models/users.model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  errorMessage: string = '';

  // สำหรับฟอร์มเพิ่มสินค้าใหม่
  newProduct: Partial<Product> = { name: '', description: '', price: 0, stock: 0 };

  // ข้อมูลฝั่ง Users
  users: Users[] = [];
  newUser: Partial<Users> = { username: '', password: '', name: '', role: '', contactInfo: '' };

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
    this.loadUsers(); // เรียกโหลดข้อมูลผู้ใช้ตอนเปิดหน้าเว็บ
  }

  // ================= MANAGING PRODUCTS =================
  loadProducts() {
    this.productService.getAll().subscribe({
      next: (data) => this.products = data,
      error: (err) => this.errorMessage = 'ไม่สามารถโหลดข้อมูลสินค้าได้'
    });
  }

  addProduct() {
    this.productService.create(this.newProduct).subscribe({
      next: () => {
        this.loadProducts();
        this.newProduct = { name: '', description: '', price: 0, stock: 0 };
      },
      error: (err) => this.errorMessage = 'ไม่สามารถเพิ่มสินค้าได้'
    });
  }

  deleteProduct(id: number) {
    this.productService.delete(id).subscribe({
      next: () => this.loadProducts(),
      error: (err) => this.errorMessage = 'ไม่สามารถลบสินค้าได้'
    });
  }

  // ================= MANAGING USERS (แก้จุดที่ขาดไป) =================
  loadUsers() {
    this.productService.getUsersAll().subscribe({
      next: (data) => this.users = data,
      error: (err) => this.errorMessage = 'ไม่สามารถโหลดข้อมูลผู้ใช้ได้'
    });
  }

  addUser() {
    this.productService.createUser(this.newUser).subscribe({
      next: () => {
        this.loadUsers(); // โหลดตารางผู้ใช้ใหม่เมื่อเพิ่มสำเร็จ
        this.newUser = { username: '', password: '', name: '', role: '', contactInfo: '' }; // ล้างค่าในฟอร์ม
      },
      error: (err) => this.errorMessage = 'ไม่สามารถเพิ่มผู้ใช้งานได้'
    });
  }

  deleteUser(id: number | undefined) {
    if (!id) return;
    this.productService.deleteUser(id).subscribe({
      next: () => this.loadUsers(), // โหลดตารางผู้ใช้ใหม่เมื่อลบสำเร็จ
      error: (err) => this.errorMessage = 'ไม่สามารถลบผู้ใช้งานได้'
    });
  }
}
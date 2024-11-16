import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EncryptionService {

  private secretKey: string = 'my_secret_key'; // Khóa bí mật

  // Mã hóa XOR
  encrypt(data: string): string {
    let encrypted = '';
    for (let i = 0; i < data.length; i++) {
      encrypted += String.fromCharCode(data.charCodeAt(i) ^ this.secretKey.charCodeAt(i % this.secretKey.length));
    }
    return btoa(encrypted); // Chuyển sang Base64
  }

  // Giải mã XOR
  decrypt(encryptedData: string): string {
    const decodedData = atob(encryptedData); // Giải mã Base64
    let decrypted = '';
    for (let i = 0; i < decodedData.length; i++) {
      decrypted += String.fromCharCode(decodedData.charCodeAt(i) ^ this.secretKey.charCodeAt(i % this.secretKey.length));
    }
    return decrypted;
  }

}

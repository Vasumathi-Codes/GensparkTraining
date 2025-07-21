import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { VideoService } from '../../services/video';

@Component({
  selector: 'app-video-upload',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './video-upload.html',
  styleUrl: './video-upload.css'
})
export class VideoUpload {
  selectedFile: File | null = null;
  title = '';
  description = '';
  uploadMessage = '';

  constructor(private videoService: VideoService) {}

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onUpload(): void {
    if (this.selectedFile) {
      const formData = new FormData();
      formData.append('file', this.selectedFile);
      formData.append('title', this.title);
      formData.append('description', this.description);

      this.videoService.uploadVideo(formData).subscribe({
        next: () => {
          this.uploadMessage = 'Upload successful!';
          this.resetForm();
        },
        error: () => {
          this.uploadMessage = 'Upload failed!';
        }
      });
    }
  }

  resetForm(): void {
    this.selectedFile = null;
    this.title = '';
    this.description = '';
  }
}

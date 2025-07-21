// src/app/services/video.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TrainingVideo } from '../models/training-video.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class VideoService {
  private baseUrl = 'http://localhost:5050/api/videos'; 

  constructor(private http: HttpClient) {}

  getVideos(): Observable<TrainingVideo[]> {
    return this.http.get<TrainingVideo[]>(this.baseUrl);
  }

  uploadVideo(formData: FormData): Observable<any> {
    return this.http.post(this.baseUrl + '/upload', formData);
  }
}

// src/app/components/video-list/video-list.component.ts
import { Component, OnInit } from '@angular/core';
import { TrainingVideo } from '../../models/training-video.model';
import { VideoService } from '../../services/video';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-video-list',
  standalone: true,
  imports: [CommonModule], 
  templateUrl: './video-list.html',
  styleUrls: ['./video-list.css']
})
export class VideoList implements OnInit {
  videos: TrainingVideo[] = [];

  constructor(private videoService: VideoService) {}

  ngOnInit(): void {
    this.videoService.getVideos().subscribe((data:TrainingVideo[]) => {
      this.videos = data;
    });
  }
}

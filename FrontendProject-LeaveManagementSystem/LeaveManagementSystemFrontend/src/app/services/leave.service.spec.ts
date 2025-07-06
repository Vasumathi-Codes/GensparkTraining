import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { LeaveService } from './leave.service';
import { LeaveRequest } from '../models/leave-request.model';
import { LeaveType } from '../models/leave-type.model';
import { LeaveAttachmentResponse } from '../models/leave-attachment.model';
import { ApiResponse } from '../models/api-response.model';
import { UserLeaveBalanceResponseDto } from '../models/user-leave-balance.model';

describe('LeaveService', () => {
  let service: LeaveService;
  let httpMock: HttpTestingController;
  const baseUrl = 'http://localhost:5000/api/v1';

  beforeEach(() => {
    localStorage.setItem('accessToken', 'test-token');
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [LeaveService]
    });
    service = TestBed.inject(LeaveService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    localStorage.clear();
    httpMock.verify();
  });

  it('should get leave types', () => {
    const mockTypes: LeaveType[] = [
      { id: 'lt1', name: 'Sick Leave', standardLeaveCount: 10 },
      { id: 'lt2', name: 'Casual Leave', standardLeaveCount: 8 }
    ];

    service.getLeaveTypes().subscribe(types => {
      expect(types.length).toBe(2);
      expect(types[0].name).toBe('Sick Leave');
    });

    const req = httpMock.expectOne(`${baseUrl}/leavetypes`);
    expect(req.request.method).toBe('GET');
    req.flush(mockTypes);
  });

  it('should apply for leave', () => {
    const request = { leaveTypeId: 'lt1', startDate: '2025-07-01' };
    service.applyLeave(request).subscribe(res => {
      expect(res.status).toBe('Pending');
    });
    const req = httpMock.expectOne(`${baseUrl}/leave-requests`);
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({ status: 'Pending' });
  });

  it('should upload attachment', () => {
    const formData = new FormData();
    formData.append('file', new Blob(['mock'], { type: 'text/plain' }), 'test.txt');

    service.uploadAttachment(formData).subscribe(res => {
      expect(res.success).toBeTrue();
    });

    const req = httpMock.expectOne(`${baseUrl}/leave-attachments`);
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');
    req.flush({ success: true });
  });

  it('should get leave history', () => {
    const mockHistory: LeaveRequest[] = [{
      id: 'lr1', userId: 'u1', userName: 'Chellz', leaveTypeId: 'lt1',
      leaveTypeName: 'Sick Leave', startDate: '2025-07-01', endDate: '2025-07-02',
      reason: 'Fever', status: 'Approved', createdAt: '', updatedAt: ''
    }];

    service.getLeaveHistory('sick', 'startDate', 'asc', 1, 10).subscribe(res => {
      expect(res[0].leaveTypeName).toBe('Sick Leave');
    });

    const req = httpMock.expectOne(`${baseUrl}/leave-requests?search=sick&sortBy=startDate&sortDirection=asc&page=1&pageSize=10`);
    expect(req.request.method).toBe('GET');
    req.flush(mockHistory);
  });

  it('should get leave request by ID', () => {
    const mockRequest: LeaveRequest = {
      id: 'lr123', userId: 'u1', userName: 'Chellz', leaveTypeId: 'lt1', leaveTypeName: 'Sick Leave',
      startDate: '2025-07-01', endDate: '2025-07-02', reason: 'Fever', status: 'Pending',
      createdAt: '', updatedAt: ''
    };

    service.getLeaveRequestById('lr123').subscribe(res => {
      expect(res.id).toBe('lr123');
    });

    const req = httpMock.expectOne(`${baseUrl}/leave-requests/lr123`);
    expect(req.request.method).toBe('GET');
    req.flush(mockRequest);
  });

  it('should get leave attachments by request ID', () => {
    const mockAttachments: ApiResponse<LeaveAttachmentResponse[]> = {
      data: [{
        id: 'a1', leaveRequestId: 'lr123', fileName: 'proof.pdf', uploadedAt: '', downloadUrl: 'http://...' 
      }]
    };

    service.getLeaveAttachmentsByRequestId('lr123').subscribe(res => {
      expect(res.data[0].fileName).toBe('proof.pdf');
    });

    const req = httpMock.expectOne(`${baseUrl}/leave-attachments/by-leave-request/lr123`);
    expect(req.request.method).toBe('GET');
    req.flush(mockAttachments);
  });

  it('should update leave status', () => {
    service.updateLeaveStatus('lr1', 'Approved').subscribe(res => {
      expect(res.updated).toBeTrue();
    });

    const req = httpMock.expectOne(`${baseUrl}/leave-requests/lr1/status`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual({ status: 'Approved' });
    req.flush({ updated: true });
  });

  it('should delete attachment', () => {
    service.deleteAttachment('att1').subscribe(res => {
      expect(res.deleted).toBeTrue();
    });

    const req = httpMock.expectOne(`${baseUrl}/leave-attachments/att1`);
    expect(req.request.method).toBe('DELETE');
    req.flush({ deleted: true });
  });

  it('should get leave balance for type', () => {
    service.getLeaveBalanceForType('u1', 'lt1').subscribe(res => {
      expect(res.available).toBe(4);
    });

    const req = httpMock.expectOne(`${baseUrl}/leave-balance/user/u1/leaveType/lt1`);
    expect(req.request.method).toBe('GET');
    req.flush({ type: 'Casual', available: 4 });
  });

  
});
export interface LeaveRequest {
  id: string;
  userId: string;
  userName: string;
  leaveTypeId: string;
  leaveTypeName: string;
  startDate: string;
  endDate: string;
  reason: string;
  status: string; 
  reviewedById?: string;
  reviewedByName?: string;
  createdAt: string;
  updatedAt: string;
}

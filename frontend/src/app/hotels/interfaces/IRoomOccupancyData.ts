export interface IRoomOccupancyData {
  date: string; // Format: YYYY-MM-DD
  roomName: string;
  occupiedCount: number;
  totalRooms: number;
  isFull: boolean;
}

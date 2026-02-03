export interface IRoomTypeOccupancy {
  typeName: string;
  total: number;
  dailyOccupancy: {
    available: number;
    percentage: number;
  }[];
}
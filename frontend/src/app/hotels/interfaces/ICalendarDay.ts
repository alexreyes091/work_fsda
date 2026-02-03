export interface ICalendarDay {
  date: Date;
  percentage: number;
  isCurrentMonth: boolean;
  isSelected?: boolean;
  occupied?: number;
  available?: number;
  total?: number;
  hotelName?: string;
}
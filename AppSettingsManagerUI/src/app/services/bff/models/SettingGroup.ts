import { Setting } from './Setting';
import { Permission } from './Permission';

export interface SettingGroup {
  id: string;
  createdBy: string;
  lastUpdatedAt: Date;
  settings: Setting[];
  permissions: Permission[];
}
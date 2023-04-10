import { PermissionLevel } from './PermissionLevel';

export interface Permission {
    userId: string;
    settingGroupId: string;
    currentPermissionLevel: PermissionLevel;
    approvedBy: string;
    waitingForApproval: boolean;
    requestedPermissionLevel: PermissionLevel;
  }
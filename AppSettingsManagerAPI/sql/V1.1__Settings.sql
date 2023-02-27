START TRANSACTION;

ALTER TABLE `BaseUsers` MODIFY COLUMN `Password` varchar(36) CHARACTER SET utf8mb4 NOT NULL;

CREATE TABLE `Settings` (
                            `Id` varchar(36) CHARACTER SET utf8mb4 NOT NULL,
                            `Version` int NOT NULL,
                            `Input` longtext CHARACTER SET utf8mb4 NOT NULL,
                            `IsCurrent` tinyint(1) NOT NULL,
                            `CreatedBy` varchar(36) CHARACTER SET utf8mb4 NOT NULL,
                            `LastUpdatedAt` timestamp(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6) ON UPDATE CURRENT_TIMESTAMP(6),
                            `CreatedAt` datetime(6) NOT NULL,
                            `BaseUserUserId` varchar(36) CHARACTER SET utf8mb4 NULL,
                            CONSTRAINT `PK_Settings` PRIMARY KEY (`Id`, `Version`),
                            CONSTRAINT `FK_Settings_BaseUsers_BaseUserUserId` FOREIGN KEY (`BaseUserUserId`) REFERENCES `BaseUsers` (`UserId`)
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Settings_BaseUserUserId` ON `Settings` (`BaseUserUserId`);

CREATE INDEX `IX_Settings_Id` ON `Settings` (`Id`);

CREATE INDEX `IX_Settings_IsCurrent` ON `Settings` (`IsCurrent`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230217190935_Settings', '6.0.10');

COMMIT;
START TRANSACTION;

ALTER TABLE `BaseUsers` ADD `Email` varchar(50) CHARACTER SET utf8mb4 NOT NULL DEFAULT '';

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20230221141212_Email', '6.0.10');

COMMIT;
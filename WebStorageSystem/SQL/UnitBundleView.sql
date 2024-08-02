CREATE VIEW [dbo].UnitBundleView
	AS 
		SELECT Units.InventoryNumber, Units.Id AS UnitId, NULL AS BundleId, 'Unit' AS TableName FROM Units
		WHERE Units.PartOfBundleId IS NULL
	UNION ALL
		SELECT Bundles.InventoryNumber, NULL AS UnitId, Bundles.Id AS BundleId, 'Bundle' AS TableName FROM Bundles
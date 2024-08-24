CREATE VIEW [dbo].UnitBundleView
	AS 
		SELECT Units.InventoryNumber, Units.Id AS UnitId, NULL AS BundleId, Units.LocationId, Units.DefaultLocationId, Units.HasDefect, 'Unit' AS TableName FROM Units
		WHERE Units.PartOfBundleId IS NULL
	UNION ALL
		SELECT Bundles.InventoryNumber, NULL AS UnitId, Bundles.Id AS BundleId, Bundles.LocationId, Bundles.DefaultLocationId, Bundles.HasDefect, 'Bundle' AS TableName FROM Bundles
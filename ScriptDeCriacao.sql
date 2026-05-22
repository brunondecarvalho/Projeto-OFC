-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema bd_esfiha
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema esfiha_bd
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema esfiha_bd
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `esfiha_bd` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `esfiha_bd` ;

-- -----------------------------------------------------
-- Table `esfiha_bd`.`role`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`role` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`user` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdRole` INT NOT NULL,
  `Name` VARCHAR(90) NOT NULL,
  `Email` VARCHAR(45) NOT NULL,
  `Password` VARCHAR(255) NOT NULL,
  `Phone` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdRole_idx` (`IdRole` ASC) VISIBLE,
  CONSTRAINT `IdRole`
    FOREIGN KEY (`IdRole`)
    REFERENCES `esfiha_bd`.`role` (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 5
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`addresses`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`addresses` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdUser` INT NOT NULL,
  `Address` VARCHAR(150) NOT NULL,
  `Number` INT NOT NULL,
  `Neighborhood` VARCHAR(45) NOT NULL,
  `CEP` INT NOT NULL,
  `Complement` VARCHAR(100) NULL DEFAULT NULL,
  `Landmark` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdUser_idx` (`IdUser` ASC) VISIBLE,
  CONSTRAINT `IdUser`
    FOREIGN KEY (`IdUser`)
    REFERENCES `esfiha_bd`.`user` (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`status`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`status` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`combos`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`combos` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdStatus` INT NOT NULL,
  `Name` VARCHAR(100) NOT NULL,
  `Description` TEXT NOT NULL,
  `Price` DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `combos_ibfk_1` (`IdStatus` ASC) VISIBLE,
  CONSTRAINT `combos_ibfk_1`
    FOREIGN KEY (`IdStatus`)
    REFERENCES `esfiha_bd`.`status` (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`productcategory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`productcategory` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`product`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`product` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdCategory` INT NOT NULL,
  `IdStatus` INT NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  `Description` VARCHAR(100) NULL DEFAULT NULL,
  `Price` DECIMAL(10,0) NOT NULL,
  `Image` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdCategory_idx` (`IdCategory` ASC) VISIBLE,
  INDEX `IdStatus_idx` (`IdStatus` ASC) VISIBLE,
  CONSTRAINT `IdCategory`
    FOREIGN KEY (`IdCategory`)
    REFERENCES `esfiha_bd`.`productcategory` (`Id`),
  CONSTRAINT `IdStatus`
    FOREIGN KEY (`IdStatus`)
    REFERENCES `esfiha_bd`.`status` (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`comboproduct`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`comboproduct` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdCombo` INT NOT NULL,
  `IdProduct` INT NOT NULL,
  `Quantity` INT NOT NULL DEFAULT '1',
  PRIMARY KEY (`Id`),
  INDEX `IdCombo` (`IdCombo` ASC) VISIBLE,
  INDEX `IdProduct` (`IdProduct` ASC) VISIBLE,
  CONSTRAINT `comboproduct_ibfk_1`
    FOREIGN KEY (`IdCombo`)
    REFERENCES `esfiha_bd`.`combos` (`Id`),
  CONSTRAINT `comboproduct_ibfk_2`
    FOREIGN KEY (`IdProduct`)
    REFERENCES `esfiha_bd`.`product` (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`discountcategory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`discountcategory` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Type` VARCHAR(12) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 6
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`discounts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`discounts` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `CouponCode` VARCHAR(40) NOT NULL,
  `IdDiscountType` INT NOT NULL,
  `DiscountValue` DECIMAL(10,2) NULL DEFAULT NULL,
  `StartDate` DATETIME NOT NULL,
  `EndDate` DATETIME NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdDiscountType_idx` (`IdDiscountType` ASC) VISIBLE,
  CONSTRAINT `IdDiscountType`
    FOREIGN KEY (`IdDiscountType`)
    REFERENCES `esfiha_bd`.`discountcategory` (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 4
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`driver`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`driver` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdUser` INT NOT NULL,
  `IdStatus` INT NOT NULL,
  `CNH` VARCHAR(90) NOT NULL,
  `LicensePlate` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdxDriverIdStatus` (`IdStatus` ASC) VISIBLE,
  INDEX `IdxDriverIdUser` (`IdUser` ASC) VISIBLE,
  CONSTRAINT `FkDriverStatus`
    FOREIGN KEY (`IdStatus`)
    REFERENCES `esfiha_bd`.`status` (`Id`),
  CONSTRAINT `FkDriverUser`
    FOREIGN KEY (`IdUser`)
    REFERENCES `esfiha_bd`.`user` (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`ordercategory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`ordercategory` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`order`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`order` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdUser` INT NOT NULL,
  `IdOrderCategory` INT NOT NULL,
  `IdAddress` INT NULL DEFAULT NULL,
  `IdStatus` INT NOT NULL,
  `SubtotalValue` DECIMAL(10,2) NOT NULL,
  `DeliveryValue` DECIMAL(10,2) NOT NULL,
  `DiscountValue` DECIMAL(10,2) NOT NULL,
  `TotalValue` DECIMAL(10,2) NOT NULL,
  `Date` DATETIME NOT NULL,
  `DeliveryTime` DECIMAL(5,2) NOT NULL,
  `Note` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdxIdOrderCategory` (`IdOrderCategory` ASC) VISIBLE,
  INDEX `IdxIdAddress` (`IdAddress` ASC) VISIBLE,
  INDEX `IdxIdUser` (`IdUser` ASC) VISIBLE,
  INDEX `IdxIdStatus` (`IdStatus` ASC) VISIBLE,
  CONSTRAINT `FkOrderAddress`
    FOREIGN KEY (`IdAddress`)
    REFERENCES `esfiha_bd`.`addresses` (`Id`),
  CONSTRAINT `FkOrderOrderCategory`
    FOREIGN KEY (`IdOrderCategory`)
    REFERENCES `esfiha_bd`.`ordercategory` (`Id`),
  CONSTRAINT `FkOrderStatus`
    FOREIGN KEY (`IdStatus`)
    REFERENCES `esfiha_bd`.`status` (`Id`),
  CONSTRAINT `FkOrderUser`
    FOREIGN KEY (`IdUser`)
    REFERENCES `esfiha_bd`.`user` (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`orderproduct`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`orderproduct` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdOrder` INT NOT NULL,
  `IdProduct` INT NOT NULL,
  `Quantity` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdOrder_idx` (`IdOrder` ASC) VISIBLE,
  INDEX `IdProduct_idx` (`IdProduct` ASC) VISIBLE,
  CONSTRAINT `IdOrder`
    FOREIGN KEY (`IdOrder`)
    REFERENCES `esfiha_bd`.`order` (`Id`),
  CONSTRAINT `IdProduct`
    FOREIGN KEY (`IdProduct`)
    REFERENCES `esfiha_bd`.`product` (`Id`))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`transactions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`transactions` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdUser` INT NOT NULL,
  `IdOrder` INT NOT NULL,
  `IdPaymentMethod` INT NOT NULL,
  `IdStatus` INT NOT NULL,
  `TotalValue` DECIMAL(10,0) NOT NULL,
  `IdGatewayTransaction` INT NOT NULL,
  `Payload` INT NOT NULL,
  `CreationDate` DATETIME NOT NULL,
  `UpdateDate` DATETIME NULL DEFAULT NULL,
  `PayloadResponse` JSON NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdUser_idx` (`IdUser` ASC) VISIBLE,
  INDEX `IdOrder_idx` (`IdOrder` ASC) VISIBLE,
  INDEX `IdStatus_idx` (`IdStatus` ASC) VISIBLE,
  CONSTRAINT `FkIdOrder`
    FOREIGN KEY (`IdOrder`)
    REFERENCES `esfiha_bd`.`order` (`Id`),
  CONSTRAINT `FkIdStatus`
    FOREIGN KEY (`IdStatus`)
    REFERENCES `esfiha_bd`.`status` (`Id`),
  CONSTRAINT `FkIdUser`
    FOREIGN KEY (`IdUser`)
    REFERENCES `esfiha_bd`.`user` (`Id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

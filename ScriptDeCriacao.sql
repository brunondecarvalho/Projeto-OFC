-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema esfiha_bd
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema esfiha_bd
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `esfiha_bd` DEFAULT CHARACTER SET utf8mb4 ;
-- -----------------------------------------------------
-- Schema bd_esfiha
-- -----------------------------------------------------
USE `esfiha_bd` ;

-- -----------------------------------------------------
-- Table `esfiha_bd`.`Role`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`Role` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`User`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`User` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdRole` INT NOT NULL,
  `Name` VARCHAR(90) NOT NULL,
  `Email` VARCHAR(45) NOT NULL,
  `Password` VARCHAR(45) NOT NULL,
  `Phone` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdRole_idx` (`IdRole` ASC) VISIBLE,
  CONSTRAINT `IdRole`
    FOREIGN KEY (`IdRole`)
    REFERENCES `esfiha_bd`.`Role` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`Address`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`Address` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdUser` INT NOT NULL,
  `Address` VARCHAR(150) NOT NULL,
  `Number` INT NOT NULL,
  `Neighborhood` VARCHAR(45) NOT NULL,
  `CEP` INT NOT NULL,
  `Complement` VARCHAR(100) NULL,
  `Landmark` VARCHAR(45) NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdUser_idx` (`IdUser` ASC) VISIBLE,
  CONSTRAINT `IdUser`
    FOREIGN KEY (`IdUser`)
    REFERENCES `esfiha_bd`.`User` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`ProductCategory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`ProductCategory` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`Status`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`Status` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`Product`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`Product` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdCategory` INT NOT NULL,
  `IdStatus` INT NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  `Description` VARCHAR(100) NULL,
  `Price` DECIMAL NOT NULL,
  `Image` VARCHAR(45) NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdCategory_idx` (`IdCategory` ASC) VISIBLE,
  INDEX `IdStatus_idx` (`IdStatus` ASC) VISIBLE,
  CONSTRAINT `IdCategory`
    FOREIGN KEY (`IdCategory`)
    REFERENCES `esfiha_bd`.`ProductCategory` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `IdStatus`
    FOREIGN KEY (`IdStatus`)
    REFERENCES `esfiha_bd`.`Status` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`OrderCategory`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`OrderCategory` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`Order`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`Order` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdUser` INT NOT NULL,
  `IdOrderCategory` INT NOT NULL,
  `IdAddress` INT NULL,
  `IdStatus` INT NOT NULL,
  `SubtotalValue` DECIMAL NOT NULL,
  `DeliveryValue` DECIMAL NOT NULL,
  `DiscountValue` DECIMAL NOT NULL,
  `TotalValue` DECIMAL NOT NULL,
  `Date` DATETIME NOT NULL,
  `DeliveryTime` DECIMAL NOT NULL,
  `Note` VARCHAR(100) NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdOrderCategory_idx` (`IdOrderCategory` ASC) VISIBLE,
  INDEX `IdAddress_idx` (`IdAddress` ASC) VISIBLE,
  INDEX `IdUser_idx` (`IdUser` ASC) VISIBLE,
  INDEX `IdStatus_idx` (`IdStatus` ASC) VISIBLE,
  CONSTRAINT `IdOrderCategory`
    FOREIGN KEY (`IdOrderCategory`)
    REFERENCES `esfiha_bd`.`OrderCategory` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `IdAddress`
    FOREIGN KEY (`IdAddress`)
    REFERENCES `esfiha_bd`.`Address` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `IdUser`
    FOREIGN KEY (`IdUser`)
    REFERENCES `esfiha_bd`.`User` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `IdStatus`
    FOREIGN KEY (`IdStatus`)
    REFERENCES `esfiha_bd`.`Status` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`OrderProduct`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`OrderProduct` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdOrder` INT NOT NULL,
  `IdProduct` INT NOT NULL,
  `Quantity` INT NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdOrder_idx` (`IdOrder` ASC) VISIBLE,
  INDEX `IdProduct_idx` (`IdProduct` ASC) VISIBLE,
  CONSTRAINT `IdOrder`
    FOREIGN KEY (`IdOrder`)
    REFERENCES `esfiha_bd`.`Order` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `IdProduct`
    FOREIGN KEY (`IdProduct`)
    REFERENCES `esfiha_bd`.`Product` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`Transactions`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`Transactions` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdUser` INT NOT NULL,
  `IdOrder` INT NOT NULL,
  `IdPaymentMethod` INT NOT NULL,
  `IdStatus` INT NOT NULL,
  `TotalValue` DECIMAL NOT NULL,
  `IdGatewayTransaction` INT NOT NULL,
  `Payload` INT NOT NULL,
  `CreationDate` DATETIME NOT NULL,
  `UpdateDate` DATETIME NULL,
  `PayloadResponse` JSON NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdUser_idx` (`IdUser` ASC) VISIBLE,
  INDEX `IdOrder_idx` (`IdOrder` ASC) VISIBLE,
  INDEX `IdStatus_idx` (`IdStatus` ASC) VISIBLE,
  CONSTRAINT `IdUser`
    FOREIGN KEY (`IdUser`)
    REFERENCES `esfiha_bd`.`User` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `IdOrder`
    FOREIGN KEY (`IdOrder`)
    REFERENCES `esfiha_bd`.`Order` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `IdStatus`
    FOREIGN KEY (`IdStatus`)
    REFERENCES `esfiha_bd`.`Status` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `esfiha_bd`.`Driver`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `esfiha_bd`.`Driver` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdUser` INT NOT NULL,
  `IdStatus` INT NOT NULL,
  `CNH` VARCHAR(90) NOT NULL,
  `LicensePlate` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`Id`),
  INDEX `IdStatus_idx` (`IdStatus` ASC) VISIBLE,
  INDEX `IdUser_idx` (`IdUser` ASC) VISIBLE,
  CONSTRAINT `IdStatus`
    FOREIGN KEY (`IdStatus`)
    REFERENCES `esfiha_bd`.`Status` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `IdUser`
    FOREIGN KEY (`IdUser`)
    REFERENCES `esfiha_bd`.`User` (`Id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

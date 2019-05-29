-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema Monitoring
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `Monitoring` ;

-- -----------------------------------------------------
-- Schema Monitoring
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Monitoring` DEFAULT CHARACTER SET utf8 ;
USE `Monitoring` ;

-- -----------------------------------------------------
-- Table `Monitoring`.`cabinets`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`cabinets` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`cabinets` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `cabinet` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Monitoring`.`hosts`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`hosts` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`hosts` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `hostname` VARCHAR(45) NOT NULL,
  `operating_system` VARCHAR(150) NOT NULL,
  `bios_version` VARCHAR(150) NOT NULL,
  `state` VARCHAR(45) NOT NULL,
  `last_update_time` VARCHAR(45) NOT NULL,
  `cabinet_id` INT NULL,
  PRIMARY KEY (`id`, `hostname`),
  CONSTRAINT `to_cabinets`
    FOREIGN KEY (`cabinet_id`)
    REFERENCES `Monitoring`.`cabinets` (`id`)
    ON DELETE SET NULL
    ON UPDATE CASCADE)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `Hostname_UNIQUE` ON `Monitoring`.`hosts` (`hostname` ASC);

CREATE UNIQUE INDEX `idHost_UNIQUE` ON `Monitoring`.`hosts` (`id` ASC);

CREATE INDEX `to_cabinets_idx` ON `Monitoring`.`hosts` (`cabinet_id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`devices`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`devices` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`devices` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `device_name_hash` INT NOT NULL,
  `device_type` VARCHAR(3) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

CREATE UNIQUE INDEX `id_UNIQUE` ON `Monitoring`.`devices` (`id` ASC);

CREATE UNIQUE INDEX `device_name_hash_UNIQUE` ON `Monitoring`.`devices` (`device_name_hash` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`host_devices`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`host_devices` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`host_devices` (
  `host_id` INT NOT NULL,
  `device_id` INT NOT NULL,
  CONSTRAINT `from_host_devices_to_hosts`
    FOREIGN KEY (`host_id`)
    REFERENCES `Monitoring`.`hosts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `from_host_devices_to_devices`
    FOREIGN KEY (`device_id`)
    REFERENCES `Monitoring`.`devices` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;

CREATE INDEX `from_host_devices_to_devices_idx` ON `Monitoring`.`host_devices` (`device_id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`programs`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`programs` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`programs` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name_version_hash` INT NOT NULL,
  `name` VARCHAR(200) NOT NULL,
  `version` VARCHAR(45) NOT NULL,
  `vendor` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

CREATE UNIQUE INDEX `id_UNIQUE` ON `Monitoring`.`programs` (`id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`host_programs`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`host_programs` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`host_programs` (
  `host_id` INT NOT NULL,
  `program_id` INT NOT NULL,
  CONSTRAINT `from_host_programs_to_hosts`
    FOREIGN KEY (`host_id`)
    REFERENCES `Monitoring`.`hosts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `from_host_programs_to_programs`
    FOREIGN KEY (`program_id`)
    REFERENCES `Monitoring`.`programs` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;

CREATE INDEX `from_host_programs_to_programs_idx` ON `Monitoring`.`host_programs` (`program_id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`processes`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`processes` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`processes` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(150) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

CREATE UNIQUE INDEX `id_UNIQUE` ON `Monitoring`.`processes` (`id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`host_processes`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`host_processes` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`host_processes` (
  `host_id` INT NOT NULL,
  `process_id` INT NOT NULL,
  CONSTRAINT `from_host_processes_to_hosts`
    FOREIGN KEY (`host_id`)
    REFERENCES `Monitoring`.`hosts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `from_host_processes_to_processes`
    FOREIGN KEY (`process_id`)
    REFERENCES `Monitoring`.`processes` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `from_host_processes_to_processes_idx` ON `Monitoring`.`host_processes` (`process_id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`host_device_history`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`host_device_history` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`host_device_history` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `host_id` INT NOT NULL,
  `device_id` INT NOT NULL,
  `action` TINYINT NOT NULL,
  `looked` TINYINT NOT NULL,
  `date` VARCHAR(30) NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `from_host_devices_history_to_hosts`
    FOREIGN KEY (`host_id`)
    REFERENCES `Monitoring`.`hosts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `from_history_to_devices`
    FOREIGN KEY (`device_id`)
    REFERENCES `Monitoring`.`devices` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;

CREATE INDEX `from_history_to_devices_idx` ON `Monitoring`.`host_device_history` (`device_id` ASC);

CREATE UNIQUE INDEX `id_UNIQUE` ON `Monitoring`.`host_device_history` (`id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`host_program_history`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`host_program_history` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`host_program_history` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `host_id` INT NOT NULL,
  `program_id` INT NOT NULL,
  `action` TINYINT NOT NULL,
  `looked` TINYINT NOT NULL,
  `date` VARCHAR(30) NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `from_host_programs_history_to_hosts`
    FOREIGN KEY (`host_id`)
    REFERENCES `Monitoring`.`hosts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `from_program_histoty_to_programs`
    FOREIGN KEY (`program_id`)
    REFERENCES `Monitoring`.`programs` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;

CREATE INDEX `from_program_histoty_to_programs_idx` ON `Monitoring`.`host_program_history` (`program_id` ASC);

CREATE UNIQUE INDEX `id_UNIQUE` ON `Monitoring`.`host_program_history` (`id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`device_mb`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`device_mb` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`device_mb` (
  `device_name_hash` INT NOT NULL,
  `manufacturer` VARCHAR(200) NOT NULL,
  `model` VARCHAR(45) NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `product` VARCHAR(45) NOT NULL,
  `serial_number` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`device_name_hash`, `manufacturer`),
  CONSTRAINT `from_mb_to_devices`
    FOREIGN KEY (`device_name_hash`)
    REFERENCES `Monitoring`.`devices` (`device_name_hash`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `name_hash_UNIQUE` ON `Monitoring`.`device_mb` (`device_name_hash` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`device_cpu`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`device_cpu` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`device_cpu` (
  `device_name_hash` INT NOT NULL,
  `manufacturer` VARCHAR(200) NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `cores` INT NOT NULL,
  `clock_speed` INT NOT NULL,
  PRIMARY KEY (`device_name_hash`),
  CONSTRAINT `from_cpu_to_devices`
    FOREIGN KEY (`device_name_hash`)
    REFERENCES `Monitoring`.`devices` (`device_name_hash`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `device_name_hash_UNIQUE` ON `Monitoring`.`device_cpu` (`device_name_hash` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`device_ram`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`device_ram` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`device_ram` (
  `device_name_hash` INT NOT NULL,
  `manufacturer` VARCHAR(200) NOT NULL,
  `clock_speed` INT NOT NULL,
  `memory_type` VARCHAR(20) NOT NULL,
  `form_factor` VARCHAR(20) NOT NULL,
  `size` BIGINT(20) NOT NULL,
  PRIMARY KEY (`device_name_hash`),
  CONSTRAINT `from_ram_to_devices`
    FOREIGN KEY (`device_name_hash`)
    REFERENCES `Monitoring`.`devices` (`device_name_hash`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `device_name_hash_UNIQUE` ON `Monitoring`.`device_ram` (`device_name_hash` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`device_hdd`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`device_hdd` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`device_hdd` (
  `device_name_hash` INT NOT NULL,
  `description` VARCHAR(45) NOT NULL,
  `caption` VARCHAR(45) NOT NULL,
  `size` BIGINT(20) NOT NULL,
  `free_space` BIGINT(20) NOT NULL,
  `file_system` VARCHAR(6) NOT NULL,
  PRIMARY KEY (`device_name_hash`),
  CONSTRAINT `from_hdd_to devices`
    FOREIGN KEY (`device_name_hash`)
    REFERENCES `Monitoring`.`devices` (`device_name_hash`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `device_name_hash_UNIQUE` ON `Monitoring`.`device_hdd` (`device_name_hash` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`device_net`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`device_net` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`device_net` (
  `device_name_hash` INT NOT NULL,
  `mac` VARCHAR(20) NOT NULL,
  `description` VARCHAR(45) NOT NULL,
  `gateway` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`device_name_hash`, `mac`),
  CONSTRAINT `from_net_to_devices`
    FOREIGN KEY (`device_name_hash`)
    REFERENCES `Monitoring`.`devices` (`device_name_hash`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `device_name_hash_UNIQUE` ON `Monitoring`.`device_net` (`device_name_hash` ASC);

CREATE UNIQUE INDEX `mac_UNIQUE` ON `Monitoring`.`device_net` (`mac` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`device_gpu`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`device_gpu` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`device_gpu` (
  `device_name_hash` INT NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `memory_size` BIGINT(20) NOT NULL,
  PRIMARY KEY (`device_name_hash`),
  CONSTRAINT `from_gpu_to_devices`
    FOREIGN KEY (`device_name_hash`)
    REFERENCES `Monitoring`.`devices` (`device_name_hash`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `device_name_hash_UNIQUE` ON `Monitoring`.`device_gpu` (`device_name_hash` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`net_ip_addresses`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`net_ip_addresses` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`net_ip_addresses` (
  `mac` VARCHAR(20) NOT NULL,
  `ip` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`mac`, `ip`),
  CONSTRAINT `to_device_net`
    FOREIGN KEY (`mac`)
    REFERENCES `Monitoring`.`device_net` (`mac`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;

CREATE INDEX `to_device_net_idx` ON `Monitoring`.`net_ip_addresses` (`mac` ASC);


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `Monitoring`.`cabinets`
-- -----------------------------------------------------
START TRANSACTION;
USE `Monitoring`;
INSERT INTO `Monitoring`.`cabinets` (`id`, `cabinet`) VALUES (1, 'кабинет 1000');
INSERT INTO `Monitoring`.`cabinets` (`id`, `cabinet`) VALUES (2, 'бухи');
INSERT INTO `Monitoring`.`cabinets` (`id`, `cabinet`) VALUES (3, 'ИТ');

COMMIT;


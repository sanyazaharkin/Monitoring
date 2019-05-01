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
-- Table `Monitoring`.`operating_systems`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`operating_systems` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`operating_systems` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `system` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`id`, `system`))
ENGINE = InnoDB;

CREATE UNIQUE INDEX `system_UNIQUE` ON `Monitoring`.`operating_systems` (`system` ASC);

CREATE UNIQUE INDEX `id_UNIQUE` ON `Monitoring`.`operating_systems` (`id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`host_states`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`host_states` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`host_states` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `description` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

CREATE UNIQUE INDEX `id_UNIQUE` ON `Monitoring`.`host_states` (`id` ASC);

CREATE UNIQUE INDEX `description_UNIQUE` ON `Monitoring`.`host_states` (`description` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`hosts`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`hosts` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`hosts` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `hostname` VARCHAR(45) NOT NULL,
  `state` INT NOT NULL,
  `operating_system` INT NOT NULL,
  `bios_version` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`id`, `hostname`),
  CONSTRAINT `to_operating_system`
    FOREIGN KEY (`operating_system`)
    REFERENCES `Monitoring`.`operating_systems` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `to_host_states`
    FOREIGN KEY (`state`)
    REFERENCES `Monitoring`.`host_states` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `Hostname_UNIQUE` ON `Monitoring`.`hosts` (`hostname` ASC);

CREATE UNIQUE INDEX `idHost_UNIQUE` ON `Monitoring`.`hosts` (`id` ASC);

CREATE INDEX `to_operating_system_idx` ON `Monitoring`.`hosts` (`operating_system` ASC);

CREATE INDEX `to_host_states_idx` ON `Monitoring`.`hosts` (`state` ASC);


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
  PRIMARY KEY (`host_id`, `device_id`),
  CONSTRAINT `from_host_devices_to_hosts`
    FOREIGN KEY (`host_id`)
    REFERENCES `Monitoring`.`hosts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `from_host_devices_to_devices`
    FOREIGN KEY (`device_id`)
    REFERENCES `Monitoring`.`devices` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `from_host_devices_to_devices_idx` ON `Monitoring`.`host_devices` (`device_id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`vendors`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`vendors` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`vendors` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `vendor` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`, `vendor`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Monitoring`.`programs`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`programs` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`programs` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name_version_hash` INT NOT NULL,
  `name` VARCHAR(200) NOT NULL,
  `version` VARCHAR(45) NOT NULL,
  `vendor_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `from_programs_to_vendors`
    FOREIGN KEY (`vendor_id`)
    REFERENCES `Monitoring`.`vendors` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `from_programs_to_vendors_idx` ON `Monitoring`.`programs` (`vendor_id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`host_programs`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`host_programs` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`host_programs` (
  `host_id` INT NOT NULL,
  `program_id` INT NOT NULL,
  PRIMARY KEY (`host_id`, `program_id`),
  CONSTRAINT `from_host_programs_to_hosts`
    FOREIGN KEY (`host_id`)
    REFERENCES `Monitoring`.`hosts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `from_host_programs_to_programs`
    FOREIGN KEY (`program_id`)
    REFERENCES `Monitoring`.`programs` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `from_host_programs_to_programs_idx` ON `Monitoring`.`host_programs` (`program_id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`processes`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`processes` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`processes` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
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
  PRIMARY KEY (`host_id`, `process_id`),
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
  `host_id` INT NOT NULL,
  `device_id` INT NOT NULL,
  `action` TINYINT NOT NULL,
  `looked` TINYINT NOT NULL,
  `date` VARCHAR(10) NOT NULL,
  `time` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`host_id`),
  CONSTRAINT `from_host_devices_history_to_hosts`
    FOREIGN KEY (`host_id`)
    REFERENCES `Monitoring`.`hosts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `from_history_to_devices`
    FOREIGN KEY (`device_id`)
    REFERENCES `Monitoring`.`devices` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `from_history_to_devices_idx` ON `Monitoring`.`host_device_history` (`device_id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`host_program_history`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`host_program_history` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`host_program_history` (
  `host_id` INT NOT NULL,
  `program_id` INT NOT NULL,
  `action` TINYINT NOT NULL,
  `looked` TINYINT NOT NULL,
  `date` VARCHAR(10) NOT NULL,
  `time` VARCHAR(10) NOT NULL,
  PRIMARY KEY (`host_id`),
  CONSTRAINT `from_host_programs_history_to_hosts`
    FOREIGN KEY (`host_id`)
    REFERENCES `Monitoring`.`hosts` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `from_program_histoty_to_programs`
    FOREIGN KEY (`program_id`)
    REFERENCES `Monitoring`.`programs` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `from_program_histoty_to_programs_idx` ON `Monitoring`.`host_program_history` (`program_id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`manufacturers`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`manufacturers` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`manufacturers` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`, `name`))
ENGINE = InnoDB;

CREATE UNIQUE INDEX `id_UNIQUE` ON `Monitoring`.`manufacturers` (`id` ASC);

CREATE UNIQUE INDEX `name_UNIQUE` ON `Monitoring`.`manufacturers` (`name` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`device_mb`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`device_mb` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`device_mb` (
  `device_name_hash` INT NOT NULL,
  `manufacturer_id` INT NOT NULL,
  `model` VARCHAR(45) NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `product` VARCHAR(45) NOT NULL,
  `serial_number` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`device_name_hash`, `manufacturer_id`),
  CONSTRAINT `to_manufacturers`
    FOREIGN KEY (`manufacturer_id`)
    REFERENCES `Monitoring`.`manufacturers` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `from_mb_to_devices`
    FOREIGN KEY (`device_name_hash`)
    REFERENCES `Monitoring`.`devices` (`device_name_hash`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `name_hash_UNIQUE` ON `Monitoring`.`device_mb` (`device_name_hash` ASC);

CREATE INDEX `to_manufacturers_idx` ON `Monitoring`.`device_mb` (`manufacturer_id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`device_cpu`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`device_cpu` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`device_cpu` (
  `device_name_hash` INT NOT NULL,
  `manufacturer_id` INT NOT NULL,
  `name` VARCHAR(45) NOT NULL,
  `threads` INT NOT NULL,
  `cores` INT NOT NULL,
  `clock_speed` INT NOT NULL,
  PRIMARY KEY (`device_name_hash`),
  CONSTRAINT `from_cpu_to_manufacturers`
    FOREIGN KEY (`manufacturer_id`)
    REFERENCES `Monitoring`.`manufacturers` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `from_cpu_to_devices`
    FOREIGN KEY (`device_name_hash`)
    REFERENCES `Monitoring`.`devices` (`device_name_hash`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `device_name_hash_UNIQUE` ON `Monitoring`.`device_cpu` (`device_name_hash` ASC);

CREATE INDEX `from_cpu_to_manufacturers_idx` ON `Monitoring`.`device_cpu` (`manufacturer_id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`form_factor`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`form_factor` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`form_factor` (
  `id` INT NOT NULL,
  `description` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

CREATE UNIQUE INDEX `id_UNIQUE` ON `Monitoring`.`form_factor` (`id` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`memory_type`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`memory_type` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`memory_type` (
  `id` INT NOT NULL,
  `desrciption` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

CREATE UNIQUE INDEX `id_UNIQUE` ON `Monitoring`.`memory_type` (`id` ASC);

CREATE UNIQUE INDEX `desrciption_UNIQUE` ON `Monitoring`.`memory_type` (`desrciption` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`device_ram`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`device_ram` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`device_ram` (
  `device_name_hash` INT NOT NULL,
  `manufacturer_id` INT NOT NULL,
  `clock_speed` INT NOT NULL,
  `voltage` INT NOT NULL,
  `memory_type` INT NOT NULL,
  `form_factor` INT NOT NULL,
  `size` BIGINT(20) NOT NULL,
  PRIMARY KEY (`device_name_hash`),
  CONSTRAINT `from_ram_to_manufacturer`
    FOREIGN KEY (`manufacturer_id`)
    REFERENCES `Monitoring`.`manufacturers` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `from_ram_to_devices`
    FOREIGN KEY (`device_name_hash`)
    REFERENCES `Monitoring`.`devices` (`device_name_hash`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `to_form_factor`
    FOREIGN KEY (`form_factor`)
    REFERENCES `Monitoring`.`form_factor` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `to_memory_type`
    FOREIGN KEY (`memory_type`)
    REFERENCES `Monitoring`.`memory_type` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `device_name_hash_UNIQUE` ON `Monitoring`.`device_ram` (`device_name_hash` ASC);

CREATE INDEX `from_ram_to_manufacturer_idx` ON `Monitoring`.`device_ram` (`manufacturer_id` ASC);

CREATE INDEX `to_form_factor_idx` ON `Monitoring`.`device_ram` (`form_factor` ASC);

CREATE INDEX `to_memory_type_idx` ON `Monitoring`.`device_ram` (`memory_type` ASC);


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
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `device_name_hash_UNIQUE` ON `Monitoring`.`device_hdd` (`device_name_hash` ASC);


-- -----------------------------------------------------
-- Table `Monitoring`.`net_gateways`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`net_gateways` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`net_gateways` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `gateway` VARCHAR(20) NOT NULL,
  PRIMARY KEY (`id`, `gateway`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Monitoring`.`device_net`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Monitoring`.`device_net` ;

CREATE TABLE IF NOT EXISTS `Monitoring`.`device_net` (
  `device_name_hash` INT NOT NULL,
  `mac` VARCHAR(20) NOT NULL,
  `description` VARCHAR(45) NOT NULL,
  `gateway_id` INT NOT NULL,
  `device_netcol` VARCHAR(45) NULL,
  PRIMARY KEY (`device_name_hash`, `mac`),
  CONSTRAINT `from_net_to_devices`
    FOREIGN KEY (`device_name_hash`)
    REFERENCES `Monitoring`.`devices` (`device_name_hash`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `from_net_to_gateways`
    FOREIGN KEY (`gateway_id`)
    REFERENCES `Monitoring`.`net_gateways` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE UNIQUE INDEX `device_name_hash_UNIQUE` ON `Monitoring`.`device_net` (`device_name_hash` ASC);

CREATE INDEX `from_net_to_gateways_idx` ON `Monitoring`.`device_net` (`gateway_id` ASC);

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
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
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
-- Data for table `Monitoring`.`form_factor`
-- -----------------------------------------------------
START TRANSACTION;
USE `Monitoring`;
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (-1, '<Null>');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (0, 'Unknown');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (1, 'Other');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (2, 'SIP');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (3, 'DIP');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (4, 'ZIP');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (5, 'SOJ');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (6, 'Proprietary');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (7, 'SIMM');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (8, 'DIMM');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (9, 'TSOP');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (10, 'PGA');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (11, 'RIMM');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (12, 'SODIMM');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (13, 'SRIMM');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (14, 'SMD');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (15, 'SSMP');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (16, 'QFP');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (17, 'TQFP');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (18, 'SOIC');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (19, 'LCC');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (20, 'PLCC');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (21, 'BGA');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (22, 'FPBGA');
INSERT INTO `Monitoring`.`form_factor` (`id`, `description`) VALUES (23, 'LGA');

COMMIT;


-- -----------------------------------------------------
-- Data for table `Monitoring`.`memory_type`
-- -----------------------------------------------------
START TRANSACTION;
USE `Monitoring`;
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (-1, '<Null>');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (0, 'Unknown');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (1, 'Other');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (2, 'DRAM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (3, 'Synchronous DRAM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (4, 'Cache DRAM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (5, 'EDO');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (6, 'EDRAM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (7, 'VRAM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (8, 'SRAM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (9, 'RAM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (10, 'ROM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (11, 'Flash');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (12, 'EEPROM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (13, 'FEPROM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (14, 'EPROM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (15, 'CDRAM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (16, '3DRAM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (17, 'SDRAM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (18, 'SGRAM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (19, 'RDRAM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (20, 'DDR');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (21, 'DDR2');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (22, 'DDR2 FB-DIMM');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (24, 'DDR3');
INSERT INTO `Monitoring`.`memory_type` (`id`, `desrciption`) VALUES (25, 'FBD2');

COMMIT;


-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 09, 2020 at 04:17 PM
-- Server version: 10.4.14-MariaDB
-- PHP Version: 7.4.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `rwdb_prk`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_memberholder`
--

CREATE TABLE `tbl_memberholder` (
  `member_id` int(11) NOT NULL,
  `cardserial` varchar(50) NOT NULL,
  `magdata` varchar(50) NOT NULL,
  `date_created` timestamp NOT NULL DEFAULT current_timestamp(),
  `created_by` int(11) NOT NULL,
  `date_last_modified` timestamp NULL DEFAULT NULL,
  `last_modified_by` int(11) DEFAULT NULL,
  `member_status` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_memberholder`
--

INSERT INTO `tbl_memberholder` (`member_id`, `cardserial`, `magdata`, `date_created`, `created_by`, `date_last_modified`, `last_modified_by`, `member_status`) VALUES
(1, '1E4EF92F', '123', '2020-12-09 10:59:51', 1, '2020-12-09 15:14:24', 1, 0),
(4, '4278190084', '3083832820373683760=23021010614506', '2020-12-09 13:34:12', 1, '2020-12-09 15:15:04', 1, 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_memberholder`
--
ALTER TABLE `tbl_memberholder`
  ADD PRIMARY KEY (`member_id`),
  ADD KEY `tbl_memberholder_createdby` (`created_by`),
  ADD KEY `tbl_memberholder_modifiedby` (`last_modified_by`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_memberholder`
--
ALTER TABLE `tbl_memberholder`
  MODIFY `member_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `tbl_memberholder`
--
ALTER TABLE `tbl_memberholder`
  ADD CONSTRAINT `tbl_memberholder_createdby` FOREIGN KEY (`created_by`) REFERENCES `tbl_user` (`userid`),
  ADD CONSTRAINT `tbl_memberholder_modifiedby` FOREIGN KEY (`last_modified_by`) REFERENCES `tbl_user` (`userid`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

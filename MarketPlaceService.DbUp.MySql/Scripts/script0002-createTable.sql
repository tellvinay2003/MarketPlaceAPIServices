CREATE TABLE IF NOT EXISTS `Publisher` (
  `id` varchar(36) NOT NULL,
  `description` varchar(45) NOT NULL, 
  `categoryype` tinyint(1) NOT NULL,
  `createdon` datetime NOT NULL,
  `modifiedon` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

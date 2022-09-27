CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE TABLE device (
        id uuid NOT NULL,
        device_public_key text NULL,
        CONSTRAINT p_k_device PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE TABLE device_registration_request (
        id uuid NOT NULL,
        device_public_key text NULL,
        created_at timestamp NOT NULL,
        CONSTRAINT p_k_device_registration_request PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE TABLE device_registration_request_state (
        id uuid NOT NULL,
        is_accepted boolean NOT NULL,
        CONSTRAINT p_k_device_registration_request_state PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE TABLE device_state (
        id uuid NOT NULL,
        device_public_key text NULL,
        device_status integer NOT NULL,
        CONSTRAINT p_k_device_state PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE TABLE facility (
        id uuid NOT NULL,
        ext_id text NULL,
        changed_at timestamp NOT NULL,
        code character varying(36) NULL,
        name character varying(255) NULL,
        CONSTRAINT p_k_facility PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE TABLE facility_access (
        id uuid NOT NULL,
        user_id uuid NOT NULL,
        facility_id uuid NOT NULL,
        CONSTRAINT p_k_facility_access PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE TABLE facility_config (
        id uuid NOT NULL,
        acceptance_process_type integer NOT NULL,
        pallet_code_prefix text NULL,
        CONSTRAINT p_k_facility_config PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE TABLE sync_position (
        id uuid NOT NULL,
        position bigint NOT NULL,
        code character varying(36) NULL,
        name character varying(255) NULL,
        CONSTRAINT p_k_sync_position PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE TABLE "user" (
        id uuid NOT NULL,
        login text NULL,
        is_locked_ext boolean NOT NULL,
        facility_ext_id text NULL,
        facility_name text NULL,
        department_ext_id text NULL,
        department_name text NULL,
        position_ext_id text NULL,
        position_name text NULL,
        ext_id text NULL,
        changed_at timestamp NOT NULL,
        code character varying(36) NULL,
        name character varying(255) NULL,
        CONSTRAINT p_k_user PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE TABLE user_config (
        id uuid NOT NULL,
        default_facility_id uuid NOT NULL,
        CONSTRAINT p_k_user_config PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE TABLE user_state (
        id uuid NOT NULL,
        is_locked boolean NOT NULL,
        CONSTRAINT p_k_user_state PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE UNIQUE INDEX "IX_device_device_public_key" ON device (device_public_key);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE UNIQUE INDEX "IX_device_registration_request_device_public_key" ON device_registration_request (device_public_key);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE UNIQUE INDEX "IX_device_registration_request_device_public_key_created_at" ON device_registration_request (device_public_key, created_at);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    CREATE UNIQUE INDEX "IX_device_state_device_public_key" ON device_state (device_public_key);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210208224045_ShemaRecreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210208224045_ShemaRecreate', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209014701_TPacLineAdded') THEN
    CREATE TABLE pac (
        id uuid NOT NULL,
        pac_date_time timestamp NOT NULL,
        facility_id text NULL,
        supplier_id text NULL,
        supplier_name text NULL,
        purchase_booking_id text NULL,
        purchase_booking_date date NOT NULL,
        purchase_id text NULL,
        purchase_date date NOT NULL,
        ext_id text NULL,
        changed_at timestamp NOT NULL,
        CONSTRAINT p_k_pac PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209014701_TPacLineAdded') THEN
    CREATE TABLE pac_line (
        id uuid NOT NULL,
        line_num integer NOT NULL,
        pac_line_id text NULL,
        product_id text NULL,
        product_name text NULL,
        product_barcode_main text NULL,
        product_unit_of_measure text NULL,
        qty_expected numeric NOT NULL,
        product_barcodes text[] NULL,
        ext_id text NULL,
        changed_at timestamp NOT NULL,
        pac_row_id uuid NULL,
        CONSTRAINT p_k_pac_line PRIMARY KEY (id),
        CONSTRAINT f_k_pac_line__pac_pac_row_id FOREIGN KEY (pac_row_id) REFERENCES pac (id) ON DELETE RESTRICT
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209014701_TPacLineAdded') THEN
    CREATE INDEX i_x_pac_line_pac_row_id ON pac_line (pac_row_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209014701_TPacLineAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210209014701_TPacLineAdded', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209030553_TPacLineFPacLineIdDrop') THEN
    ALTER TABLE pac_line DROP COLUMN pac_line_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209030553_TPacLineFPacLineIdDrop') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210209030553_TPacLineFPacLineIdDrop', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209183850_TPacLineFKTPac') THEN
    ALTER TABLE pac_line DROP CONSTRAINT f_k_pac_line__pac_pac_row_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209183850_TPacLineFKTPac') THEN
    DROP INDEX i_x_pac_line_pac_row_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209183850_TPacLineFKTPac') THEN
    ALTER TABLE pac_line DROP COLUMN pac_row_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209183850_TPacLineFKTPac') THEN
    ALTER TABLE pac_line ADD pac_id uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209183850_TPacLineFKTPac') THEN
    CREATE INDEX i_x_pac_line_pac_id ON pac_line (pac_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209183850_TPacLineFKTPac') THEN
    ALTER TABLE pac_line ADD CONSTRAINT f_k_pac_line__pac_pac_id FOREIGN KEY (pac_id) REFERENCES pac (id) ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209183850_TPacLineFKTPac') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210209183850_TPacLineFKTPac', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209232948_TPacToTPacHeadRenamed') THEN
    ALTER TABLE pac_line DROP CONSTRAINT f_k_pac_line__pac_pac_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209232948_TPacToTPacHeadRenamed') THEN
    DROP TABLE pac;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209232948_TPacToTPacHeadRenamed') THEN
    DROP INDEX i_x_pac_line_pac_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209232948_TPacToTPacHeadRenamed') THEN
    ALTER TABLE pac_line ADD pac_head_id uuid NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209232948_TPacToTPacHeadRenamed') THEN
    CREATE TABLE pac_head (
        id uuid NOT NULL,
        pac_date_time timestamp NOT NULL,
        facility_id text NULL,
        supplier_id text NULL,
        supplier_name text NULL,
        purchase_booking_id text NULL,
        purchase_booking_date date NOT NULL,
        purchase_id text NULL,
        purchase_date date NOT NULL,
        ext_id text NULL,
        changed_at timestamp NOT NULL,
        CONSTRAINT p_k_pac_head PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209232948_TPacToTPacHeadRenamed') THEN
    CREATE INDEX i_x_pac_line_pac_head_id ON pac_line (pac_head_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209232948_TPacToTPacHeadRenamed') THEN
    ALTER TABLE pac_line ADD CONSTRAINT f_k_pac_line_pac_head_pac_head_id FOREIGN KEY (pac_head_id) REFERENCES pac_head (id) ON DELETE RESTRICT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209232948_TPacToTPacHeadRenamed') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210209232948_TPacToTPacHeadRenamed', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209233457_TPacToTPacHeadRenamed2') THEN
    ALTER TABLE pac_line DROP CONSTRAINT f_k_pac_line_pac_head_pac_head_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209233457_TPacToTPacHeadRenamed2') THEN
    ALTER TABLE pac_line DROP COLUMN pac_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209233457_TPacToTPacHeadRenamed2') THEN
    ALTER TABLE pac_line ALTER COLUMN pac_head_id SET NOT NULL;
    ALTER TABLE pac_line ALTER COLUMN pac_head_id SET DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209233457_TPacToTPacHeadRenamed2') THEN
    ALTER TABLE pac_line ADD CONSTRAINT f_k_pac_line_pac_head_pac_head_id FOREIGN KEY (pac_head_id) REFERENCES pac_head (id) ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210209233457_TPacToTPacHeadRenamed2') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210209233457_TPacToTPacHeadRenamed2', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210001824_TPurchaseTaskPlus') THEN
    CREATE SEQUENCE purchase_task_code_seq START WITH 1 INCREMENT BY 1 NO MINVALUE NO MAXVALUE NO CYCLE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210001824_TPurchaseTaskPlus') THEN
    CREATE TABLE purchase_task_head (
        id uuid NOT NULL,
        CONSTRAINT p_k_purchase_task_head PRIMARY KEY (id)
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210001824_TPurchaseTaskPlus') THEN
    CREATE TABLE purchase_task_line (
        id uuid NOT NULL,
        purchase_task_head_id uuid NOT NULL,
        CONSTRAINT p_k_purchase_task_line PRIMARY KEY (id),
        CONSTRAINT f_k_purchase_task_line_purchase_task_head_purchase_task_head_id FOREIGN KEY (purchase_task_head_id) REFERENCES purchase_task_head (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210001824_TPurchaseTaskPlus') THEN
    CREATE TABLE purchase_task_pallet (
        id uuid NOT NULL,
        purchase_task_head_id uuid NOT NULL,
        CONSTRAINT p_k_purchase_task_pallet PRIMARY KEY (id),
        CONSTRAINT "f_k_purchase_task_pallet_purchase_task_head_purchase_task_head_~" FOREIGN KEY (purchase_task_head_id) REFERENCES purchase_task_head (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210001824_TPurchaseTaskPlus') THEN
    CREATE TABLE purchase_task_line_state (
        id uuid NOT NULL,
        purchase_task_line_id uuid NOT NULL,
        CONSTRAINT p_k_purchase_task_line_state PRIMARY KEY (id),
        CONSTRAINT "f_k_purchase_task_line_state_purchase_task_line_purchase_task_l~" FOREIGN KEY (purchase_task_line_id) REFERENCES purchase_task_line (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210001824_TPurchaseTaskPlus') THEN
    CREATE INDEX i_x_purchase_task_line_purchase_task_head_id ON purchase_task_line (purchase_task_head_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210001824_TPurchaseTaskPlus') THEN
    CREATE UNIQUE INDEX i_x_purchase_task_line_state_purchase_task_line_id ON purchase_task_line_state (purchase_task_line_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210001824_TPurchaseTaskPlus') THEN
    CREATE INDEX i_x_purchase_task_pallet_purchase_task_head_id ON purchase_task_pallet (purchase_task_head_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210001824_TPurchaseTaskPlus') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210210001824_TPurchaseTaskPlus', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_line_state ADD expiration_date timestamp NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_line_state ADD expiration_days_plus bigint NOT NULL DEFAULT 0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_line_state ADD qty_broken numeric NOT NULL DEFAULT 0.0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_line_state ADD qty_normal numeric NOT NULL DEFAULT 0.0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_line ADD item_abc text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_line ADD item_barcodes text[] NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_line ADD item_ext_id text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_line ADD item_name text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_line ADD price numeric NOT NULL DEFAULT 0.0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_line ADD quantity numeric NOT NULL DEFAULT 0.0;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_head ADD code character varying(36) NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_head ADD facility_id uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    CREATE INDEX i_x_purchase_task_head_facility_id ON purchase_task_head (facility_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    ALTER TABLE purchase_task_head ADD CONSTRAINT f_k_purchase_task_head_facility_facility_id FOREIGN KEY (facility_id) REFERENCES facility (id) ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003238_TPurchaseTaskFields') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210210003238_TPurchaseTaskFields', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003431_TPurchaseTaskPalletFields') THEN
    ALTER TABLE purchase_task_pallet ADD abc text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003431_TPurchaseTaskPalletFields') THEN
    ALTER TABLE purchase_task_pallet ADD code text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003431_TPurchaseTaskPalletFields') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210210003431_TPurchaseTaskPalletFields', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003852_TPurchaseTaskLineFieldsAdd') THEN
    ALTER TABLE purchase_task_head ADD changed_at timestamp NOT NULL DEFAULT TIMESTAMP '1970-01-01T00:00:00Z';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003852_TPurchaseTaskLineFieldsAdd') THEN
    ALTER TABLE purchase_task_head ADD created_at timestamp NOT NULL DEFAULT TIMESTAMP '1970-01-01T00:00:00Z';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003852_TPurchaseTaskLineFieldsAdd') THEN
    ALTER TABLE purchase_task_head ADD ext_id text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210003852_TPurchaseTaskLineFieldsAdd') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210210003852_TPurchaseTaskLineFieldsAdd', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210021236_TPurchaseTaskLinePriceRemove') THEN
    ALTER TABLE purchase_task_line DROP COLUMN price;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210021236_TPurchaseTaskLinePriceRemove') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210210021236_TPurchaseTaskLinePriceRemove', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210024621_TPurchaseTaskLineItemToProduct') THEN
    ALTER TABLE purchase_task_line RENAME COLUMN item_name TO product_name;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210024621_TPurchaseTaskLineItemToProduct') THEN
    ALTER TABLE purchase_task_line RENAME COLUMN item_ext_id TO product_ext_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210024621_TPurchaseTaskLineItemToProduct') THEN
    ALTER TABLE purchase_task_line RENAME COLUMN item_barcodes TO product_barcodes;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210024621_TPurchaseTaskLineItemToProduct') THEN
    ALTER TABLE purchase_task_line RENAME COLUMN item_abc TO product_abc;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210024621_TPurchaseTaskLineItemToProduct') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210210024621_TPurchaseTaskLineItemToProduct', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210030331_TPurchaseTaskPalletsPacHeads') THEN
    CREATE TABLE purchase_task_pac_head (
        id uuid NOT NULL,
        purchase_task_head_id uuid NOT NULL,
        CONSTRAINT p_k_purchase_task_pac_head PRIMARY KEY (id),
        CONSTRAINT "f_k_purchase_task_pac_head_purchase_task_head_purchase_task_hea~" FOREIGN KEY (purchase_task_head_id) REFERENCES purchase_task_head (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210030331_TPurchaseTaskPalletsPacHeads') THEN
    CREATE INDEX i_x_purchase_task_pac_head_purchase_task_head_id ON purchase_task_pac_head (purchase_task_head_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210030331_TPurchaseTaskPalletsPacHeads') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210210030331_TPurchaseTaskPalletsPacHeads', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210030550_TPurchaseTaskPacHeadsPlus') THEN
    ALTER TABLE purchase_task_pac_head ADD pac_head_id uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210030550_TPurchaseTaskPacHeadsPlus') THEN
    CREATE INDEX i_x_purchase_task_pac_head_pac_head_id ON purchase_task_pac_head (pac_head_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210030550_TPurchaseTaskPacHeadsPlus') THEN
    ALTER TABLE purchase_task_pac_head ADD CONSTRAINT f_k_purchase_task_pac_head_pac_head_pac_head_id FOREIGN KEY (pac_head_id) REFERENCES pac_head (id) ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210030550_TPurchaseTaskPacHeadsPlus') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210210030550_TPurchaseTaskPacHeadsPlus', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210043104_TPacLineProductAbcAdd') THEN
    ALTER TABLE pac_line ADD product_abc text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210043104_TPacLineProductAbcAdd') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210210043104_TPacLineProductAbcAdd', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210112147_TPurchaseTaskChangedAtAdd') THEN
    ALTER TABLE purchase_task_pallet ADD changed_at timestamp NOT NULL DEFAULT TIMESTAMP '1970-01-01T00:00:00Z';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210112147_TPurchaseTaskChangedAtAdd') THEN
    ALTER TABLE purchase_task_line_state ADD changed_at timestamp NOT NULL DEFAULT TIMESTAMP '1970-01-01T00:00:00Z';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210112147_TPurchaseTaskChangedAtAdd') THEN
    ALTER TABLE purchase_task_line ADD changed_at timestamp NOT NULL DEFAULT TIMESTAMP '1970-01-01T00:00:00Z';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210210112147_TPurchaseTaskChangedAtAdd') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210210112147_TPurchaseTaskChangedAtAdd', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210214193714_TFacilityConfigFIsAcceptanceByPapersEnabledAdd') THEN
    ALTER TABLE facility_config ADD is_acceptance_by_papers_enabled boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210214193714_TFacilityConfigFIsAcceptanceByPapersEnabledAdd') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210214193714_TFacilityConfigFIsAcceptanceByPapersEnabledAdd', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210214225612_TFacilityAccessFIsManual') THEN
    ALTER TABLE facility_access ADD is_manual boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210214225612_TFacilityAccessFIsManual') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210214225612_TFacilityAccessFIsManual', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210218043949_TPacStateAdded') THEN
    CREATE TABLE pac_state (
        id uuid NOT NULL,
        is_busy boolean NOT NULL,
        is_processed boolean NOT NULL,
        pac_head_id uuid NOT NULL,
        CONSTRAINT p_k_pac_state PRIMARY KEY (id),
        CONSTRAINT f_k_pac_state_pac_head_pac_head_id FOREIGN KEY (pac_head_id) REFERENCES pac_head (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210218043949_TPacStateAdded') THEN
    CREATE UNIQUE INDEX i_x_pac_state_pac_head_id ON pac_state (pac_head_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210218043949_TPacStateAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210218043949_TPacStateAdded', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210218074714_TPurchaseTaskUser') THEN
    CREATE TABLE purchase_task_user (
        id uuid NOT NULL,
        user_id uuid NOT NULL,
        purchase_task_head_id uuid NOT NULL,
        CONSTRAINT p_k_purchase_task_user PRIMARY KEY (id),
        CONSTRAINT f_k_purchase_task_user__user_user_id FOREIGN KEY (user_id) REFERENCES "user" (id) ON DELETE CASCADE,
        CONSTRAINT f_k_purchase_task_user_purchase_task_head_purchase_task_head_id FOREIGN KEY (purchase_task_head_id) REFERENCES purchase_task_head (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210218074714_TPurchaseTaskUser') THEN
    CREATE INDEX i_x_purchase_task_user_purchase_task_head_id ON purchase_task_user (purchase_task_head_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210218074714_TPurchaseTaskUser') THEN
    CREATE INDEX i_x_purchase_task_user_user_id ON purchase_task_user (user_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210218074714_TPurchaseTaskUser') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210218074714_TPurchaseTaskUser', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210218162954_TPurchaseTaskHeadFState') THEN
    ALTER TABLE purchase_task_head ADD purchase_task_state integer NOT NULL DEFAULT (6000);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210218162954_TPurchaseTaskHeadFState') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210218162954_TPurchaseTaskHeadFState', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210218204815_TPacStateFIsExported') THEN
    ALTER TABLE pac_state ADD is_exported boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210218204815_TPacStateFIsExported') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210218204815_TPacStateFIsExported', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220122840_TPurchaseTaskFCreatedByUserId') THEN
    ALTER TABLE purchase_task_head ADD created_by_user_id uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220122840_TPurchaseTaskFCreatedByUserId') THEN
    CREATE INDEX i_x_purchase_task_head_created_by_user_id ON purchase_task_head (created_by_user_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220122840_TPurchaseTaskFCreatedByUserId') THEN
    ALTER TABLE purchase_task_head ADD CONSTRAINT f_k_purchase_task_head__user_created_by_user_id FOREIGN KEY (created_by_user_id) REFERENCES "user" (id) ON DELETE CASCADE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220122840_TPurchaseTaskFCreatedByUserId') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210220122840_TPurchaseTaskFCreatedByUserId', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220192858_TPurchaseTaskLinePalletedStateAdded') THEN
    CREATE TABLE purchase_task_line_palleted_state (
        id uuid NOT NULL,
        expiration_date timestamp NULL,
        expiration_days_plus bigint NOT NULL,
        qty_normal numeric NOT NULL,
        qty_broken numeric NOT NULL,
        changed_at timestamp NOT NULL,
        purchase_task_line_id uuid NOT NULL,
        CONSTRAINT p_k_purchase_task_line_palleted_state PRIMARY KEY (id),
        CONSTRAINT "f_k_purchase_task_line_palleted_state__purchase_task_line_purchase~" FOREIGN KEY (purchase_task_line_id) REFERENCES purchase_task_line (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220192858_TPurchaseTaskLinePalletedStateAdded') THEN
    CREATE INDEX i_x_purchase_task_line_palleted_state_purchase_task_line_id ON purchase_task_line_palleted_state (purchase_task_line_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210220192858_TPurchaseTaskLinePalletedStateAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210220192858_TPurchaseTaskLinePalletedStateAdded', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210221100835_TPurchaseTaskLinePalletedStateFFsAdded') THEN
    ALTER TABLE purchase_task_line_palleted_state ADD pallet_abc text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210221100835_TPurchaseTaskLinePalletedStateFFsAdded') THEN
    ALTER TABLE purchase_task_line_palleted_state ADD pallet_code text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210221100835_TPurchaseTaskLinePalletedStateFFsAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210221100835_TPurchaseTaskLinePalletedStateFFsAdded', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222001655_TPacLineStateAdded') THEN
    CREATE TABLE pac_line_state (
        id uuid NOT NULL,
        expiration_date timestamp NULL,
        expiration_days_plus bigint NOT NULL,
        qty_normal numeric NOT NULL,
        qty_broken numeric NOT NULL,
        changed_at timestamp NOT NULL,
        pac_line_id uuid NOT NULL,
        CONSTRAINT p_k_pac_line_state PRIMARY KEY (id),
        CONSTRAINT f_k_pac_line_state_pac_line_pac_line_id FOREIGN KEY (pac_line_id) REFERENCES pac_line (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222001655_TPacLineStateAdded') THEN
    CREATE UNIQUE INDEX i_x_pac_line_state_pac_line_id ON pac_line_state (pac_line_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222001655_TPacLineStateAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210222001655_TPacLineStateAdded', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222022543_FMinorChanges') THEN
    ALTER TABLE purchase_task_line_state ALTER COLUMN expiration_days_plus TYPE integer;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222022543_FMinorChanges') THEN
    ALTER TABLE purchase_task_line_palleted_state ALTER COLUMN expiration_days_plus TYPE integer;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222022543_FMinorChanges') THEN
    ALTER TABLE pac_line_state ALTER COLUMN expiration_days_plus TYPE integer;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222022543_FMinorChanges') THEN
    ALTER TABLE pac_line_state ADD pallet_code text NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210222022543_FMinorChanges') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210222022543_FMinorChanges', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210228185751_TPurchaseTaskLineUpdateRowAdded') THEN
    CREATE TABLE purchase_task_line_update (
        id uuid NOT NULL,
        purchase_task_head_id uuid NOT NULL,
        purchase_task_line_id uuid NOT NULL,
        user_id uuid NOT NULL,
        item_barcode text NULL,
        current_pallet_code text NULL,
        purchase_task_line_update_type integer NOT NULL,
        expiration_date timestamp NULL,
        expiration_days_plus integer NOT NULL,
        qty_normal numeric NOT NULL,
        qty_broken numeric NOT NULL,
        CONSTRAINT p_k_purchase_task_line_update PRIMARY KEY (id),
        CONSTRAINT f_k_purchase_task_line_update__user_user_id FOREIGN KEY (user_id) REFERENCES "user" (id) ON DELETE CASCADE,
        CONSTRAINT "f_k_purchase_task_line_update_purchase_task_head_purchase_task_~" FOREIGN KEY (purchase_task_head_id) REFERENCES purchase_task_head (id) ON DELETE CASCADE,
        CONSTRAINT "f_k_purchase_task_line_update_purchase_task_line_purchase_task_~" FOREIGN KEY (purchase_task_line_id) REFERENCES purchase_task_line (id) ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210228185751_TPurchaseTaskLineUpdateRowAdded') THEN
    CREATE INDEX i_x_purchase_task_line_update_purchase_task_head_id ON purchase_task_line_update (purchase_task_head_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210228185751_TPurchaseTaskLineUpdateRowAdded') THEN
    CREATE INDEX i_x_purchase_task_line_update_purchase_task_line_id ON purchase_task_line_update (purchase_task_line_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210228185751_TPurchaseTaskLineUpdateRowAdded') THEN
    CREATE INDEX i_x_purchase_task_line_update_user_id ON purchase_task_line_update (user_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210228185751_TPurchaseTaskLineUpdateRowAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210228185751_TPurchaseTaskLineUpdateRowAdded', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210228214018_TPurchaseTaskHeadFStartedAtOAdded') THEN
    ALTER TABLE purchase_task_head ADD started_at timestamp NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210228214018_TPurchaseTaskHeadFStartedAtOAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210228214018_TPurchaseTaskHeadFStartedAtOAdded', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210301041958_TPacHeadFStartedAtFResponsiblePersonIdOAdded') THEN
    ALTER TABLE pac_head ADD responsible_user_id uuid NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210301041958_TPacHeadFStartedAtFResponsiblePersonIdOAdded') THEN
    ALTER TABLE pac_head ADD started_at timestamp NULL;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210301041958_TPacHeadFStartedAtFResponsiblePersonIdOAdded') THEN
    CREATE INDEX i_x_pac_head_responsible_user_id ON pac_head (responsible_user_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210301041958_TPacHeadFStartedAtFResponsiblePersonIdOAdded') THEN
    ALTER TABLE pac_head ADD CONSTRAINT f_k_pac_head__user_responsible_user_id FOREIGN KEY (responsible_user_id) REFERENCES "user" (id) ON DELETE RESTRICT;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210301041958_TPacHeadFStartedAtFResponsiblePersonIdOAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210301041958_TPacHeadFStartedAtFResponsiblePersonIdOAdded', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210301171138_TPurchaseTaskHeadFIsPubliclyAvailableOAdded') THEN
    ALTER TABLE purchase_task_head ADD is_publicly_available boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210301171138_TPurchaseTaskHeadFIsPubliclyAvailableOAdded') THEN
    CREATE INDEX "IX_purchase_task_head_is_publicly_available" ON purchase_task_head (is_publicly_available);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210301171138_TPurchaseTaskHeadFIsPubliclyAvailableOAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210301171138_TPurchaseTaskHeadFIsPubliclyAvailableOAdded', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210302061012_TPurchaseTaskLineUpdateFProductBarcodeOAddedInsteadOf') THEN
    ALTER TABLE purchase_task_line_update RENAME COLUMN item_barcode TO product_barcode;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210302061012_TPurchaseTaskLineUpdateFProductBarcodeOAddedInsteadOf') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210302061012_TPurchaseTaskLineUpdateFProductBarcodeOAddedInsteadOf', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210302083006_TExpirationDateOTypeChanged') THEN
    ALTER TABLE purchase_task_line_update ALTER COLUMN expiration_date TYPE date;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210302083006_TExpirationDateOTypeChanged') THEN
    ALTER TABLE purchase_task_line_state ALTER COLUMN expiration_date TYPE date;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210302083006_TExpirationDateOTypeChanged') THEN
    ALTER TABLE purchase_task_line_palleted_state ALTER COLUMN expiration_date TYPE date;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210302083006_TExpirationDateOTypeChanged') THEN
    ALTER TABLE pac_line_state ALTER COLUMN expiration_date TYPE date;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210302083006_TExpirationDateOTypeChanged') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210302083006_TExpirationDateOTypeChanged', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210302100415_TPurchaseTaskLineUpdateRowFCreatedAtOAdded') THEN
    ALTER TABLE purchase_task_line_update ADD created_at timestamp NOT NULL DEFAULT TIMESTAMP '1970-01-01T00:00:00Z';
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210302100415_TPurchaseTaskLineUpdateRowFCreatedAtOAdded') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210302100415_TPurchaseTaskLineUpdateRowFCreatedAtOAdded', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210324115734_TPacLineFStateFStates') THEN
    DROP INDEX i_x_pac_line_state_pac_line_id;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210324115734_TPacLineFStateFStates') THEN
    CREATE INDEX i_x_pac_line_state_pac_line_id ON pac_line_state (pac_line_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210324115734_TPacLineFStateFStates') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210324115734_TPacLineFStateFStates', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210327191850_TPurchaseTaskHeadFIsExported') THEN
    ALTER TABLE purchase_task_head ADD is_exported boolean NOT NULL DEFAULT FALSE;
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210327191850_TPurchaseTaskHeadFIsExported') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210327191850_TPurchaseTaskHeadFIsExported', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    CREATE INDEX "IX_user_changed_at" ON "user" (changed_at);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    CREATE INDEX "IX_user_ext_id" ON "user" (ext_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    CREATE INDEX "IX_sync_position_name" ON sync_position (name);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    CREATE INDEX "IX_pac_state_is_busy" ON pac_state (is_busy);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    CREATE INDEX "IX_pac_state_is_exported" ON pac_state (is_exported);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    CREATE INDEX "IX_pac_state_is_processed" ON pac_state (is_processed);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    CREATE INDEX "IX_pac_line_changed_at" ON pac_line (changed_at);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    CREATE INDEX "IX_pac_line_ext_id" ON pac_line (ext_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    CREATE INDEX "IX_pac_head_changed_at" ON pac_head (changed_at);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    CREATE INDEX "IX_pac_head_ext_id" ON pac_head (ext_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    CREATE INDEX "IX_facility_changed_at" ON facility (changed_at);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    CREATE INDEX "IX_facility_ext_id" ON facility (ext_id);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210329143224_AddSomeIndexes') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210329143224_AddSomeIndexes', '5.0.4');
    END IF;
END $$;
COMMIT;

START TRANSACTION;


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210405101758_AddSomeIndexes2') THEN
    CREATE INDEX "IX_purchase_task_head_code" ON purchase_task_head (code);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210405101758_AddSomeIndexes2') THEN
    CREATE INDEX "IX_pac_head_pac_date_time" ON pac_head (pac_date_time);
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210405101758_AddSomeIndexes2') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20210405101758_AddSomeIndexes2', '5.0.4');
    END IF;
END $$;
COMMIT;


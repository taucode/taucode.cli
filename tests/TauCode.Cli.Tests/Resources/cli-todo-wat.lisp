; Migrate metadata (mm)
; e.g.: mm --conn="Server=.;Database=econera.diet.tracking;Trusted_Connection=True;" --provider=sqlserver --to=sqlite --target-path=c:/temp/mysqlite.json -v

(defblock :name mm :is-top t
	(sub-command :value "mm")
	(idle :name args)
	(alt
		(key-with-value
			:alias connection
			:key-names "--conn" "-c"
			:key-values (choice :classes string :values *)
			:is-single t
			:is-mandatory t)

		(key-with-value
			:alias provider
			:key-names "--provider" "-p"
			:key-values (choice :classes term :values "sqlserver" "postgresql")
			:is-single t
			:is-mandatory t)

		(key
			:alias verbose
			:key-names "--verbose" "-v")

		(key-with-value
			:alias target-provider
			:key-names "--to" "-t"
			:key-values (choice :classes term :values "sqlite")
			:is-single t
			:is-mandatory t)

		(key-with-value
			:alias target-path
			:key-names "--target-path" "-tp"
			:key-values (choice :classes term key string path :values *)
			:is-single t
			:is-mandatory t)
	)
	(idle :links args next)
	(end)
)

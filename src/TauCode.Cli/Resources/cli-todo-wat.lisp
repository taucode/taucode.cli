; Migrate metadata (mm)
; e.g.: mm --conn="Server=.;Database=econera.diet.tracking;Trusted_Connection=True;" --provider=sqlserver --to=sqlite --target=c:/temp/mysqlite.json

(defblock :name mm :is-top t
	(exact-text :classes term :value "mm" :name command-mm)
	(idle :name args)
	(alt
		(key-with-value
			:alias connection
			:key-names "--conn" "-c"
			:key-values any-string
			:is-single t
			:is-mandatory t)

		(key-with-value
			:alias provider
			:key-names "--provider" "-p"
			:key-values exact-term "sqlserver" "postgresql"
			:is-single t
			:is-mandatory t)

		(key
			:alias verbose
			:key-names "--verbose" "-v")

		(key-with-value
			:alias target-provider
			:key-names "--to" "-t"
			:key-values exact-term "sqlite"
			:is-single t
			:is-mandatory t)

		(key-with-value
			:alias target-path
			:key-names "--target-path" "-tp"
			:key-values any-path
			:is-single t
			:is-mandatory t)
	)
	(idle :links args next)
	(end)
)

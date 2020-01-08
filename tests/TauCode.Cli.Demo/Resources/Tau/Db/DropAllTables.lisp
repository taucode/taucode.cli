(defblock :name drop-all-tables :is-top t
	(worker
		:worker-name drop-all-tables
		:verbs "drop-all-tables" "dat"
		:doc "Drop all tables of a database."
		:usage-samples (
			"drop-all-tables --conn Server=.;Database=my_db;Trusted_Connection=True; --provider sqlserver --exclude table_1 --exclude table_2"
			"dat -c Server=some-host;Database=my_db; -p postgresql -e table_1 -e table_2"
			))
	(idle :name args)
	(alt
		(key-value-pair
			:alias connection
			:key-names "--conn" "-c"
			:key-values (choice :classes string path :values *))

		(key-value-pair
			:alias provider
			:key-names "--provider" "-p"
			:key-values (choice :classes term :values "sqlserver" "postgresql"))

		(key-value-pair
			:alias exclude
			:key-names "--exclude" "-e"
			:key-values (choice :classes string term :values *))

	)
	(idle :links args next)
	(end)
)

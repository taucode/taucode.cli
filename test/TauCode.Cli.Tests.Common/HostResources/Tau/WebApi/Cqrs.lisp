(defblock :name serialize-data :is-top t
	(executor
		:executor-name serialize-data
		:verbs "serialize-data" "sd"
		:doc "Serializes all tables' data."
		:usage-samples (
			"sd --conn Server=.;Database=my_db;Trusted_Connection=True; --provider sqlserver --file c:/temp/my.json"
			"serialize-data -c Server=some-host;Database=my_db; -p postgresql -f c:/work/another.json"
			))
	(idle :name args)
	(alt
		(key-value-pair
			:alias connection
			:key-names "--conn" "-c"
			:key-values (choice :classes string path :values *)
			:is-single t
			:is-mandatory t)

		(key-value-pair
			:alias provider
			:key-names "--provider" "-p"
			:key-values (choice :classes term :values "sqlserver" "postgresql")
			:is-single t
			:is-mandatory t)

		(key-value-pair
			:alias file
			:key-names "--file" "-f"
			:key-values (choice :classes term key string path :values *)
			:is-single t
			:is-mandatory t)
	)
	(idle :links args next)
	(end)
)

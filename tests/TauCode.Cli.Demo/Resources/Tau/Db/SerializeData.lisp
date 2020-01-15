(defblock :name serialize-data :is-top t
	(worker
		:worker-name serialize-data
		:verbs "serialize-data" "sd"
		:doc "Serializes all tables' data."
		:usage-samples (
			"sd --conn Server=.;Database=my_db;Trusted_Connection=True; --provider sqlserver --file c:/temp/my.json"
			"serialize-data -c Server=some-host;Database=my_db; -p postgresql -f c:/work/another.json"
			))
	(idle :name args)
	(alt
		(seq
			(multi-text :classes key :values "-c" "--connection" :alias connection)
			(some-text :classes path)
		)
		(seq
			(multi-text :classes key :values "-p" "--provider" :alias provider)
			(multi-text :classes term :values "sqlserver" "postgresql")
		)
		(seq
			(multi-text :classes key :values "-f" "--file" :alias file)
			(some-text :classes path)
		)
	)
	(idle :links args next)
	(end)
)

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
		(seq
			(multi-text
				:classes key
				:values "-c" "--connection"
				:alias connection
				:action key)
			(some-text
				:classes path
				:action value)
		)
		(seq
			(multi-text
				:classes key
				:values "-p" "--provider"
				:alias provider
				:action key)
			(multi-text
				:classes term
				:values "sqlserver" "postgresql"
				:action value)
		)
		(seq
			(multi-text
				:classes key
				:values "-e" "--exclude"
				:alias exclude
				:action key)
			(some-text
				:classes string term				
				:action value)
		)
	)
	(idle :links args next)
	(end)
)

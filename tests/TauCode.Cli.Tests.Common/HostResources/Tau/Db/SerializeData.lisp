(defblock :name serialize-data :is-top t
	(worker
		:worker-name serialize-data
		:verb "sd"
		:description "Serialize data"
		:usage-samples (
			"sd -p sqlserver -e table1 --exclude table2 Server=.;Database=mydb;Trusted_Connection=True;"
			"sd --provider postgresql 'Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;'"
		)
	)

	(idle :name keys)
	(alt
		(seq
			(multi-text
				:classes key
				:values "-p" "--provider"
				:alias provider
				:action key
				:is-mandatory t)
			(multi-text
				:classes term
				:values "sqlserver" "postgresql"
				:action value
				:description "DB provider identifier"
				:doc-subst "db provider"
				)
		)
		(seq
			(multi-text
				:classes key
				:values "-e" "--exclude"
				:alias exclude-table
				:action key
				:allows-multiple t)
			(some-text
				:classes term string
				:action value
				:description "Table to exclude from serializing"
				:doc-subst "table to exclude")
		)
		(fallback :name bad-option-or-key)
	)

	(idle :links keys next)

	(some-text
		:classes path string
		:alias connection-string
		:action argument		
		:description "DB connection string to use"
		:doc-subst "connection string")

	(idle :name options)
	(opt
		(alt
			(multi-text
				:classes key
				:values "-v" "--verbose"
				:alias verbose
				:action option
				:description "Verbose output")

			(multi-text
				:classes key
				:values "-q" "--quiet"
				:alias quiet
				:action option
				:description "Don't show output")
		)
	)
	(idle :links options next)

	(end)
)

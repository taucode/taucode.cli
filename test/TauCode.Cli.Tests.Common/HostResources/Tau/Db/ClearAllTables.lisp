(defblock :name clear-all-tables :is-top t
	(executor
		:executor-name clear-all-tables
		:verb "clear-all-tables"
		:description "Clears all tables in a database"
		:usage-samples (
			"tau db clear-all-tables -p sqlserver -e table1 --exclude table2 Server=.;Database=mydb;Trusted_Connection=True;"
			"tau db clear-all-tables --provider postgresql 'Server=127.0.0.1;Port=5432;Database=myDataBase;User Id=myUsername;Password=myPassword;'"
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
				:values "sqlserver" "postgresql" "mysql"
				:action value
				:description "DB provider identifier"
				:doc-subst "db provider")
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
				:description "Table to exclude from being cleared"
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
		(multi-text
			:classes key
			:values "-v" "--verbose"
			:alias verbose
			:action option
			:description "Verbose output")
	)
	(idle :links options next)

	(end)
)
